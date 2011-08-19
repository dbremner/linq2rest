// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text.RegularExpressions;

	internal class FilterExpressionFactory : IFilterExpressionFactory
	{
		private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US");
		private static readonly Regex StringRx = new Regex(@"^'([^']*?)'$", RegexOptions.Compiled);
		private static readonly Regex FunctionRx = new Regex(@"^([^\(\)]+)\((.+)\)$");
		private static readonly Regex FunctionContentRx = new Regex(@"^(.*\((?>[^()]+|\((?<Depth>.*)|\)(?<-Depth>.*))*(?(Depth)(?!))\)|.*?)\s*,\s*(.+)$", RegexOptions.Compiled);
		private static readonly ExpressionTokenizer Tokenizer;

		static FilterExpressionFactory()
		{
			Tokenizer = new ExpressionTokenizer();
		}

		public Expression<Func<T, bool>> Create<T>(string filter)
		{
			return Create<T>(filter, DefaultCulture);
		}

		public Expression<Func<T, bool>> Create<T>(string filter, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(filter))
			{
				return x => true;
			}

			var parameter = Expression.Parameter(typeof(T), "x");

			var expression = CreateExpression<T>(filter, parameter, null, formatProvider);

			return expression == null ? x => true : Expression.Lambda<Func<T, bool>>(expression, parameter);
		}

		private TokenSet GetFunctionTokens(string filter)
		{
			Contract.Requires(filter != null);

			var functionMatch = FunctionRx.Match(filter);
			if (!functionMatch.Success)
			{
				return null;
			}

			var functionName = functionMatch.Groups[1].Value;
			var functionContent = functionMatch.Groups[2].Value;
			var functionContentMatch = FunctionContentRx.Match(functionContent);
			if (!functionContentMatch.Success)
			{
				return new FunctionTokenSet
					{
						Operation = functionName,
						Left = functionContent
					};
			}

			return new FunctionTokenSet
				{
					Operation = functionName,
					Left = functionContentMatch.Groups[1].Value,
					Right = functionContentMatch.Groups[2].Value
				};
		}

		private Expression CreateExpression<T>(string filter, ParameterExpression parameter, Type type, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(filter))
			{
				return null;
			}

			var tokens = Tokenizer.GetTokens(filter).ToArray();

			Expression existing = null;
			string combiner = null;

			if (tokens.Any())
			{
				foreach (var tokenSet in tokens)
				{
					if (string.IsNullOrWhiteSpace(tokenSet.Left))
					{
						if (string.Equals(tokenSet.Operation, "not", StringComparison.OrdinalIgnoreCase))
						{
							var right = CreateExpression<T>(tokenSet.Right, parameter, type ?? GetExpressionType<T>(tokenSet, parameter), formatProvider);

							if (right == null)
							{
								return null;
							}

							return GetOperation(tokenSet.Operation, null, right);
						}

						combiner = tokenSet.Operation;
					}
					else
					{
						var left = CreateExpression<T>(tokenSet.Left, parameter, type ?? GetExpressionType<T>(tokenSet, parameter), formatProvider);
						var right = CreateExpression<T>(tokenSet.Right, parameter, left.Type, formatProvider);

						if (right == null)
						{
							return null;
						}

						if (existing != null && !string.IsNullOrWhiteSpace(combiner))
						{
							var current = GetOperation(tokenSet.Operation, left, right);
							existing = GetOperation(combiner, existing, current);
						}
						else
						{
							existing = GetOperation(tokenSet.Operation, left, right);
						}
					}
				}

				return existing;
			}

			Expression expression = null;
			var stringMatch = StringRx.Match(filter);

			if (stringMatch.Success)
			{
				expression = Expression.Constant(stringMatch.Groups[1].Value, typeof(string));
			}
			if (expression == null)
			{
				expression = GetFunctionExpression<T>(filter, parameter, type, formatProvider);
			}
			if (expression == null)
			{
				expression = GetPropertyExpression<T>(filter, parameter);
			}
			if (expression == null)
			{
				Contract.Assume(type != null);

				expression = Expression.Constant(Convert.ChangeType(filter, type, formatProvider), type);
			}

			return expression;
		}

		private Expression GetFunctionExpression<T>(string filter, ParameterExpression parameter, Type type, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);

			var functionTokens = GetFunctionTokens(filter);
			if (functionTokens == null)
			{
				return null;
			}

			var left = CreateExpression<T>(
				functionTokens.Left,
				parameter,
				type ?? GetExpressionType<T>(functionTokens, parameter),
				formatProvider);

			var right = CreateExpression<T>(functionTokens.Right, parameter, GetFunctionParameterType(functionTokens.Operation) ?? left.Type, formatProvider);

			return GetFunction(functionTokens.Operation, left, right);
		}

		private Type GetFunctionParameterType(string operation)
		{
			Contract.Requires(operation != null);

			switch (operation.ToLowerInvariant())
			{
				case "substring":
					return typeof(int);
				default:
					return null;
			}
		}

		private Expression GetPropertyExpression<T>(string propertyToken, ParameterExpression parameter)
		{
			Contract.Requires(propertyToken != null);

			var tokens = Tokenizer.GetTokens(propertyToken);
			foreach (var token in tokens)
			{
				var expression = GetPropertyExpression<T>(token.Left, parameter) ?? GetPropertyExpression<T>(token.Right, parameter);
				return expression;
			}

			var parentType = typeof(T);
			Expression propertyExpression = null;

			var propertyChain = propertyToken.Split('/');
			foreach (var propertyName in propertyChain)
			{
				var property = parentType.GetProperty(propertyName);
				if (property != null)
				{
					parentType = property.PropertyType;
					propertyExpression = propertyExpression == null
					? Expression.Property(parameter, property)
					: Expression.Property(propertyExpression, property);
				}
			}

			return propertyExpression;
		}

		private Type GetExpressionType<T>(TokenSet set, ParameterExpression parameter)
		{
			Contract.Requires(set != null);

			var property = GetPropertyExpression<T>(set.Left, parameter) ?? GetPropertyExpression<T>(set.Right, parameter);

			return property == null ? null : property.Type;
		}

		private Expression GetOperation(string token, Expression left, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(right != null);

			token = token.ToLowerInvariant();

			if (string.Equals("not", token, StringComparison.OrdinalIgnoreCase))
			{
				return GetRightOperation(token, right);
			}

			Contract.Assume(left != null);

			return GetLeftRightOperation(token, left, right);
		}

		private Expression GetLeftRightOperation(string token, Expression left, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(left != null);
			Contract.Requires(right != null);

			switch (token.ToLowerInvariant())
			{
				case "eq":
					return Expression.Equal(left, right);
				case "ne":
					return Expression.NotEqual(left, right);
				case "gt":
					return Expression.GreaterThan(left, right);
				case "ge":
					return Expression.GreaterThanOrEqual(left, right);
				case "lt":
					return Expression.LessThan(left, right);
				case "le":
					return Expression.LessThanOrEqual(left, right);
				case "and":
					return Expression.AndAlso(left, right);
				case "or":
					return Expression.OrElse(left, right);
				case "add":
					return Expression.Add(left, right);
				case "sub":
					return Expression.Subtract(left, right);
				case "mul":
					return Expression.Multiply(left, right);
				case "div":
					return Expression.Divide(left, right);
				case "mod":
					return Expression.Modulo(left, right);
			}

			throw new InvalidOperationException("Unsupported operation");
		}

		private Expression GetRightOperation(string token, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(right != null);

			switch (token.ToLowerInvariant())
			{
				case "not":
					return Expression.Not(right);
			}

			throw new InvalidOperationException("Unsupported operation");
		}

		private Expression GetFunction(string function, Expression left, Expression right)
		{
			Contract.Requires(function != null);

			switch (function.ToLowerInvariant())
			{
				case "substringof":
					return Expression.GreaterThan(
						Expression.Call(right, MethodProvider.IndexOfMethod, new[] { left, MethodProvider.IgnoreCaseExpression }),
						Expression.Constant(-1, typeof(int)));
				case "endswith":
					return Expression.Call(left, MethodProvider.EndsWithMethod, new[] { right, MethodProvider.IgnoreCaseExpression });
				case "startswith":
					return Expression.Call(left, MethodProvider.StartsWithMethod, new[] { right, MethodProvider.IgnoreCaseExpression });
				case "length":
					return Expression.Property(left, MethodProvider.LengthProperty);
				case "indexof":
					return Expression.Call(left, MethodProvider.IndexOfMethod, new[] { right, MethodProvider.IgnoreCaseExpression });
				case "substring":
					return Expression.Call(left, MethodProvider.SubstringMethod, new[] { right });
				case "tolower":
					return Expression.Call(left, MethodProvider.ToLowerMethod);
				case "toupper":
					return Expression.Call(left, MethodProvider.ToUpperMethod);
				case "trim":
					return Expression.Call(left, MethodProvider.TrimMethod);
				case "hour":
					return Expression.Property(left, MethodProvider.HourProperty);
				case "minute":
					return Expression.Property(left, MethodProvider.MinuteProperty);
				case "second":
					return Expression.Property(left, MethodProvider.SecondProperty);
				case "day":
					return Expression.Property(left, MethodProvider.DayProperty);
				case "month":
					return Expression.Property(left, MethodProvider.MonthProperty);
				case "year":
					return Expression.Property(left, MethodProvider.YearProperty);
				case "round":
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleRoundMethod : MethodProvider.DecimalRoundMethod, left);
				case "floor":
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleFloorMethod : MethodProvider.DecimalFloorMethod, left);
				case "ceiling":
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleCeilingMethod : MethodProvider.DecimalCeilingMethod, left);
				default:
					return null;
			}


			/*
string replace(string p0, string find, string replace)
http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=replace(CompanyName, ' ', '') eq 'AlfredsFutterkiste'
string substring(string p0, int pos, int length)
http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substring(CompanyName, 1, 2) eq 'lf'
string concat(string p0, string p1)
http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=concat(concat(City, ', '), Country) eq 'Berlin, Germany'

Type Functions
bool IsOf(type p0)
http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof('NorthwindModel.Order')
bool IsOf(expression p0, type p1)
http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof(ShipCountry, 'Edm.String')
			 */
		}
	}

	internal class FunctionTokenSet : TokenSet
	{
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Operation, Left, Right);
		}
	}
}