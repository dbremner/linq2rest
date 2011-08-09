namespace UrlQueryParser
{
	using System;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text.RegularExpressions;

	public class FilterExpressionFactory : IFilterExpressionFactory
	{
		private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US");
		private static readonly Regex StringRx = new Regex(@"^'([^']*?)'$", RegexOptions.Compiled);
		private static readonly Regex FunctionRx = new Regex(@"^([^\(\)]+)\((.+)\)$");
		private static readonly Regex FunctionContentRx = new Regex(@"^(.*\((?>[^()]+|\((?<Depth>.*)|\)(?<-Depth>.*))*(?(Depth)(?!))\)|.*?)\s*,\s*(.+)$", RegexOptions.Compiled);

		private static readonly ConstantExpression IgnoreCaseExpression;
		private static readonly MethodInfo IndexOfMethod;
		private static readonly MethodInfo EndsWithMethod;
		private static readonly MethodInfo StartsWithMethod;
		private static readonly PropertyInfo LengthProperty;
		private static readonly MethodInfo SubstringMethod;
		private static readonly MethodInfo ToLowerMethod;
		private static readonly MethodInfo ToUpperMethod;
		private static readonly MethodInfo TrimMethod;
		private static readonly ExpressionTokenizer Tokenizer;

		static FilterExpressionFactory()
		{
			Type stringType = typeof(string);

			Tokenizer = new ExpressionTokenizer();
			IgnoreCaseExpression = Expression.Constant(StringComparison.OrdinalIgnoreCase);

			IndexOfMethod = stringType.GetMethod("IndexOf", new[] { stringType, typeof(StringComparison) });
			EndsWithMethod = stringType.GetMethod("EndsWith", new[] { stringType, typeof(StringComparison) });
			StartsWithMethod = stringType.GetMethod("StartsWith", new[] { stringType, typeof(StringComparison) });
			LengthProperty = stringType.GetProperty("Length", Type.EmptyTypes);
			SubstringMethod = stringType.GetMethod("Substring", new[] { typeof(int) });
			ToLowerMethod = stringType.GetMethod("ToLowerInvariant", Type.EmptyTypes);
			ToUpperMethod = stringType.GetMethod("ToUpperInvariant", Type.EmptyTypes);
			TrimMethod = stringType.GetMethod("Trim", Type.EmptyTypes);
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
				for (int i = 0; i < tokens.Length; i++)
				{
					var tokenSet = tokens[i];
					if (string.IsNullOrWhiteSpace(tokenSet.Left))
					{
						combiner = tokenSet.Operation;
					}
					else
					{
						var left = CreateExpression<T>(tokenSet.Left, parameter, type ?? GetExpressionType<T>(tokenSet, parameter), formatProvider);
						var right = CreateExpression<T>(tokenSet.Right, parameter, left.Type, formatProvider);
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
				expression = Expression.Constant(Convert.ChangeType(filter, type, formatProvider), type);
			}

			return expression;
		}

		private Expression GetFunctionExpression<T>(string filter, ParameterExpression parameter, Type type, IFormatProvider formatProvider)
		{
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
			var property = GetPropertyExpression<T>(set.Left, parameter) ?? GetPropertyExpression<T>(set.Right, parameter);

			return property == null ? null : property.Type;
		}

		private Expression GetOperation(string token, Expression left, Expression right)
		{
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
				case "not":
					return Expression.Not(right);
				case "add":
					return Expression.Add(left, right);
				case "min":
					return Expression.Subtract(left, right);
				case "mul":
					return Expression.Multiply(left, right);
				case "div":
					return Expression.Divide(left, right);
				case "mod":
					return Expression.Modulo(left, right);
				default:
					return null;
			}
		}

		private Expression GetFunction(string function, Expression left, Expression right)
		{
			switch (function.ToLowerInvariant())
			{
				case "substringof":
					return Expression.GreaterThan(
						Expression.Call(right, IndexOfMethod, new[] { left, IgnoreCaseExpression }),
						Expression.Constant(-1, typeof(int)));
				case "endswith":
					return Expression.Call(left, EndsWithMethod, new[] { right, IgnoreCaseExpression });
				case "startswith":
					return Expression.Call(left, StartsWithMethod, new[] { right, IgnoreCaseExpression });
				case "length":
					return Expression.Property(left, LengthProperty);
				case "indexof":
					return Expression.Call(left, IndexOfMethod, new[] { right, IgnoreCaseExpression });
				case "substring":
					return Expression.Call(left, SubstringMethod, new[] { right });
				case "tolower":
					return Expression.Call(left, ToLowerMethod);
				case "toupper":
					return Expression.Call(left, ToUpperMethod);
				case "trim":
					return Expression.Call(left, TrimMethod);
				default:
					return null;
			}
		}
	}

	internal class FunctionTokenSet : TokenSet
	{
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Operation, Left, Right);
		}
	}


	internal class TokenSet
	{
		public string Left { get; set; }

		public string Operation { get; set; }

		public string Right { get; set; }

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Left, Operation, Right);
		}
	}
}