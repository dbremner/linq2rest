// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Collections.Generic;

namespace Linq2Rest.Parser
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using Linq2Rest.Parser.Readers;

	/// <summary>
	/// Defines the FilterExpressionFactory.
	/// </summary>
	public class FilterExpressionFactory : IFilterExpressionFactory
	{
		private static readonly Regex StringRx = new Regex(@"^[""']([^""']*?)[""']$", RegexOptions.Compiled);
		private static readonly Regex NegateRx = new Regex(@"^-[^\d]*", RegexOptions.Compiled);
		private static readonly Regex NewRx = new Regex(@"^new (?<type>[^\(\)]+)\((?<parameters>.*)\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// Creates a filter expression from its string representation.
		/// </summary>
		/// <param name="filter">The string representation of the filter.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item to filter.</typeparam>
		/// <returns>An <see cref="Expression{TDelegate}"/> if the passed filter is valid, otherwise null.</returns>
		public Expression<Func<T, bool>> Create<T>(string filter)
		{
			return Create<T>(filter, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Creates a filter expression from its string representation.
		/// </summary>
		/// <param name="filter">The string representation of the filter.</param>
		/// <param name="formatProvider">The <see cref="IFormatProvider"/> to use when reading the filter.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item to filter.</typeparam>
		/// <returns>An <see cref="Expression{TDelegate}"/> if the passed filter is valid, otherwise null.</returns>
		public Expression<Func<T, bool>> Create<T>(string filter, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(filter))
			{
				return x => true;
			}

			var parameter = Expression.Parameter(typeof(T), "x");

			var expression = CreateExpression<T>(filter, parameter, new List<ParameterExpression>(), null, formatProvider);

			return expression == null ? x => false : Expression.Lambda<Func<T, bool>>(expression, parameter);
		}

		private static string[] GetConstructorTokens(string filter)
		{
			Contract.Requires(filter != null);

			var constructorMatch = NewRx.Match(filter);
			if (!constructorMatch.Success)
			{
				return null;
			}

			var matchGroup = constructorMatch.Groups["parameters"];

			Contract.Assume(matchGroup != null, "Otherwise match is not success.");

			var constructorContent = matchGroup.Value;
			return constructorContent.Split(',').Select(x => x.Trim().Trim(')', '(')).ToArray();
		}

		private static Type GetFunctionParameterType(string operation)
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

		private static Expression GetPropertyExpression<T>(string propertyToken, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters)
		{
			Contract.Requires(propertyToken != null);
			Contract.Requires(parameter != null);
			Contract.Requires(lambdaParameters != null);

			if (!propertyToken.IsImpliedBoolean())
			{
				var token = propertyToken.GetTokens().FirstOrDefault();
				if (token != null)
				{
					return GetPropertyExpression<T>(token.Left, parameter, lambdaParameters) ?? GetPropertyExpression<T>(token.Right, parameter, lambdaParameters);
				}
			}

			var parentType = parameter.Type;
			Expression propertyExpression = null;

			var propertyChain = propertyToken.Split('/');

			Contract.Assert(propertyChain != null);

			if (propertyChain.Any() && lambdaParameters.Any(p => p.Name == propertyChain.First()))
			{
				ParameterExpression lambdaParameter = lambdaParameters.First(p => p.Name == propertyChain.First());

				Contract.Assume(lambdaParameter != null);

				parentType = lambdaParameter.Type;
				propertyExpression = lambdaParameter;
			}

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

		private static Type GetExpressionType<T>(TokenSet set, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters)
		{
			Contract.Requires(parameter != null);
			Contract.Requires(lambdaParameters != null);

			if (set == null)
			{
				return null;
			}

			if (Regex.IsMatch(set.Left, @"^\(.*\)$") && set.Operation.IsCombinationOperation())
			{
				return null;
			}

			var property = GetPropertyExpression<T>(set.Left, parameter, lambdaParameters) ?? GetPropertyExpression<T>(set.Right, parameter, lambdaParameters);
			if (property != null)
			{
				return property.Type;
			}

			var type = GetExpressionType<T>(set.Left.GetArithmeticToken(), parameter, lambdaParameters);

			return type ?? GetExpressionType<T>(set.Right.GetArithmeticToken(), parameter, lambdaParameters);
		}

		private static Expression GetOperation(string token, Expression left, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(right != null);

			return left == null ? GetRightOperation(token, right) : GetLeftRightOperation(token, left, right);
		}

		private static Expression GetLeftRightOperation(string token, Expression left, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(left != null);
			Contract.Requires(right != null);
			try
			{
				switch (token.ToLowerInvariant())
				{
					case "eq":
						if (left.Type.IsEnum && left.Type.GetCustomAttributes(typeof(FlagsAttribute), true).Any())
						{
							var underlyingType = Enum.GetUnderlyingType(left.Type);
							var leftValue = Expression.Convert(left, underlyingType);
							var rightValue = Expression.Convert(right, underlyingType);
							var andExpression = Expression.And(leftValue, rightValue);
							return Expression.Equal(andExpression, rightValue);
						}

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

				return null;
			}
			catch
			{
				return null;
			}
		}

		private static Expression GetRightOperation(string token, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(right != null);

			switch (token.ToLowerInvariant())
			{
				case "not":
					return right.Type == typeof(bool) ? Expression.Not(right) : null;
			}

			return null;
		}

		private static Expression GetFunction(string function, Expression left, Expression right, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters)
		{
			Contract.Requires(function != null);
			Contract.Requires(left != null);

			switch (function.ToLowerInvariant())
			{
				case "substringof":
					return Expression.Call(right, MethodProvider.ContainsMethod, new[] { left });
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
					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleRoundMethod : MethodProvider.DecimalRoundMethod, left);
				case "floor":
					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleFloorMethod : MethodProvider.DecimalFloorMethod, left);
				case "ceiling":
					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleCeilingMethod : MethodProvider.DecimalCeilingMethod, left);
				case "any":
				case "all":
					{
						Contract.Assume(right != null);
						Contract.Assume(!string.IsNullOrEmpty(function));

						return CreateAnyAllExpression(
													  left,
													  right,
													  sourceParameter,
													  lambdaParameters,
													  MethodProvider.GetAnyAllMethod(function.Capitalize(), left.Type));
					}

				default:
					return null;
			}
		}

		private static Expression CreateAnyAllExpression(
			Expression left,
			Expression right,
			ParameterExpression sourceParameter,
			IEnumerable<ParameterExpression> lambdaParameters,
			MethodInfo anyAllMethod)
		{
			Contract.Requires(left != null);
			Contract.Requires(right != null);

			var genericFunc = typeof(Func<,>)
				.MakeGenericType(
								 MethodProvider.GetIEnumerableImpl(left.Type).GetGenericArguments()[0],
								 typeof(bool));

			var filteredParameters = new ParameterVisitor()
				.GetParameters(right)
				.Where(p => p.Name != sourceParameter.Name)
				.ToArray();

			if (filteredParameters.Length > 0)
			{
				return Expression.Call(
									   anyAllMethod,
									   left,
									   Expression.Lambda(genericFunc, right, filteredParameters));
			}

			return Expression.Call(
								   MethodProvider.GetAnyAllMethod("All", left.Type),
								   left,
								   Expression.Lambda(genericFunc, right, lambdaParameters));
		}

		private static Type GetNonNullableType(Type type)
		{
			Contract.Requires(type != null);

			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
					? type.GetGenericArguments()[0]
					: type;
		}

		private static bool SupportsNegate(Type type)
		{
			Contract.Requires(type != null);

			type = GetNonNullableType(type);
			if (!type.IsEnum)
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Double:
					case TypeCode.Single:
						return true;
				}
			}

			return false;
		}

		private static Expression GetParseExpression(string filter, IFormatProvider formatProvider, Type type)
		{
			Contract.Requires(type != null);

			var parseMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(x => x.Name == "Parse").ToArray();
			if (parseMethods.Length > 0)
			{
				var withFormatProvider =
					parseMethods.FirstOrDefault(
						x =>
						{
							var parameters = x.GetParameters();
							return parameters.Length == 2
								&& typeof(string).IsAssignableFrom(parameters[0].ParameterType)
								&& typeof(IFormatProvider).IsAssignableFrom(parameters[1].ParameterType);
						});
				if (withFormatProvider != null)
				{
					return Expression.Call(withFormatProvider, Expression.Constant(filter), Expression.Constant(formatProvider));
				}

				var withoutFormatProvider = parseMethods.FirstOrDefault(
						x =>
						{
							var parameters = x.GetParameters();
							return parameters.Length == 1
								&& typeof(string).IsAssignableFrom(parameters[0].ParameterType);
						});

				if (withoutFormatProvider != null)
				{
					return Expression.Call(withoutFormatProvider, Expression.Constant(filter));
				}
			}

			return null;
		}

		private Expression CreateExpression<T>(string filter, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);
			Contract.Requires(sourceParameter != null);
			Contract.Requires(lambdaParameters != null);

			if (string.IsNullOrWhiteSpace(filter))
			{
				return null;
			}

			var tokens = filter.GetTokens().ToArray();

			if (tokens.Any())
			{
				return GetTokenExpression<T>(sourceParameter, lambdaParameters, type, formatProvider, tokens);
			}

			Expression expression = null;
			var stringMatch = StringRx.Match(filter);

			if (stringMatch.Success)
			{
				expression = Expression.Constant(stringMatch.Groups[1].Value, typeof(string));
			}

			if (NegateRx.IsMatch(filter))
			{
				var negateExpression = CreateExpression<T>(
					filter.Substring(1),
					sourceParameter,
					lambdaParameters,
					type,
					formatProvider);

				Contract.Assume(negateExpression != null);
				
				expression = SupportsNegate(negateExpression.Type) ? Expression.Negate(negateExpression) : null;
			}

			if (expression == null)
			{
				expression = GetAnyAllFunctionExpression<T>(filter, sourceParameter, lambdaParameters, formatProvider);
			}

			if (expression == null)
			{
				expression = GetPropertyExpression<T>(filter, sourceParameter, lambdaParameters);
			}

			if (expression == null)
			{
				expression = GetArithmeticExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

			if (expression == null)
			{
				expression = GetFunctionExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

			if (expression == null)
			{
				if (type != null)
				{
					expression = ParameterValueReader.Read(type, filter, formatProvider);
				}
				else
				{
					var booleanExpression = (ConstantExpression)ParameterValueReader.Read(typeof(bool), filter, formatProvider);
					if (booleanExpression.Value != null)
					{
						expression = booleanExpression;
					}
				}
			}

			if (expression == null && type != null)
			{
				expression = typeof(IConvertible).IsAssignableFrom(type)
								? Expression.Constant(Convert.ChangeType(filter, type, formatProvider), type)
								: GetParseExpression(filter, formatProvider, type);
			}

			if (expression == null)
			{
				expression = GetConstructorExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

			return expression;
		}

		private Expression GetTokenExpression<T>(ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider, TokenSet[] tokens)
		{
			Contract.Requires(tokens != null);
			Contract.Requires(parameter != null);
			Contract.Requires(lambdaParameters != null);

			string combiner = null;
			Expression existing = null;
			foreach (var tokenSet in tokens)
			{
				if (string.IsNullOrWhiteSpace(tokenSet.Left))
				{
					if (string.Equals(tokenSet.Operation, "not", StringComparison.OrdinalIgnoreCase))
					{
						var right = CreateExpression<T>(
														tokenSet.Right,
														parameter,
														lambdaParameters,
														type ?? GetExpressionType<T>(tokenSet, parameter, lambdaParameters),
														formatProvider);

						return right == null
								? null
								: GetOperation(tokenSet.Operation, null, right);
					}

					combiner = tokenSet.Operation;
				}
				else
				{
					var left = CreateExpression<T>(
												   tokenSet.Left,
												   parameter,
												   lambdaParameters,
												   type ?? GetExpressionType<T>(tokenSet, parameter, lambdaParameters),
												   formatProvider);
					if (left == null)
					{
						return null;
					}

					var rightExpressionType = tokenSet.Operation == "and" ? null : left.Type;
					var right = CreateExpression<T>(tokenSet.Right, parameter, lambdaParameters, rightExpressionType, formatProvider);

					if (existing != null && !string.IsNullOrWhiteSpace(combiner))
					{
						var current = right == null ? null : GetOperation(tokenSet.Operation, left, right);
						existing = GetOperation(combiner, existing, current ?? left);
					}
					else if (right != null)
					{
						existing = GetOperation(tokenSet.Operation, left, right);
					}
				}
			}

			return existing;
		}

		private Expression GetArithmeticExpression<T>(string filter, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);
			Contract.Requires(parameter != null);
			Contract.Requires(lambdaParameters != null);

			var arithmeticToken = filter.GetArithmeticToken();
			if (arithmeticToken == null)
			{
				return null;
			}

			var type1 = type ?? GetExpressionType<T>(arithmeticToken, parameter, lambdaParameters);
			var leftExpression = CreateExpression<T>(arithmeticToken.Left, parameter, lambdaParameters, type1, formatProvider);
			var rightExpression = CreateExpression<T>(arithmeticToken.Right, parameter, lambdaParameters, type1, formatProvider);

			return leftExpression == null || rightExpression == null
					? null
					: GetLeftRightOperation(arithmeticToken.Operation, leftExpression, rightExpression);
		}

		private Expression GetConstructorExpression<T>(string filter, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters, Type resultType, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);

			if (resultType == null)
			{
				return null;
			}

			var newMatch = NewRx.Match(filter);
			if (newMatch.Success)
			{
				var matchGroup = newMatch.Groups["type"];

				Contract.Assume(matchGroup != null, "Otherwise match is not success.");

				var typeName = matchGroup.Value;
				var assemblies = AppDomain.CurrentDomain.GetAssemblies();
				var type = assemblies
					.SelectMany(x => x.GetTypes().Where(t => t.Name == typeName))
					.FirstOrDefault();

				if (type == null)
				{
					return null;
				}

				var constructorTokens = GetConstructorTokens(filter);

				var constructorInfos = type.GetConstructors().Where(x => x.GetParameters().Length == constructorTokens.Length);
				foreach (var constructorInfo in constructorInfos)
				{
					try
					{
						var parameterExpressions = constructorInfo
							.GetParameters()
							.Select((p, i) => CreateExpression<T>(constructorTokens[i], parameter, lambdaParameters, p.ParameterType, formatProvider))
							.ToArray();

						return Expression.Convert(Expression.New(constructorInfo, parameterExpressions), resultType);
					}
					catch
					{
						return null;
					}
				}
			}

			return null;
		}

		private Expression GetAnyAllFunctionExpression<T>(string filter, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);
			Contract.Requires(sourceParameter != null);
			Contract.Requires(lambdaParameters != null);

			var functionTokens = filter.GetAnyAllFunctionTokens();
			if (functionTokens == null)
			{
				return null;
			}

			var leftType = GetPropertyExpression<T>(functionTokens.Left, sourceParameter, lambdaParameters).Type;
			var left = CreateExpression<T>(
				functionTokens.Left,
				sourceParameter,
				lambdaParameters,
				leftType,
				formatProvider);

			// Create a new ParameterExpression from the lambda parameter and add to a collection to pass around
			var parameterName = functionTokens.Right.Substring(0, functionTokens.Right.IndexOf(":")).Trim();
			var lambdaParameter =
				Expression.Parameter(MethodProvider.GetIEnumerableImpl(leftType).GetGenericArguments()[0], parameterName);
			lambdaParameters.Add(lambdaParameter);
			var lambdaFilter = functionTokens.Right.Substring(functionTokens.Right.IndexOf(":") + 1).Trim();
			var lambdaType = GetFunctionParameterType(functionTokens.Operation) ?? left.Type;

			var isLambdaAnyAllFunction = lambdaFilter.GetAnyAllFunctionTokens() != null;
			var right = isLambdaAnyAllFunction
				? GetAnyAllFunctionExpression<T>(lambdaFilter, lambdaParameter, lambdaParameters, formatProvider)
				: CreateExpression<T>(lambdaFilter, sourceParameter, lambdaParameters, lambdaType, formatProvider);

			return left == null
				? null
				: GetFunction(functionTokens.Operation, left, right, sourceParameter, lambdaParameters);
		}

		private Expression GetFunctionExpression<T>(string filter, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider)
		{
			Contract.Requires(filter != null);
			Contract.Requires(sourceParameter != null);
			Contract.Requires(lambdaParameters != null);

			var functionTokens = filter.GetFunctionTokens();
			if (functionTokens == null)
			{
				return null;
			}

			var left = CreateExpression<T>(
				functionTokens.Left,
				sourceParameter,
				lambdaParameters,
				type ?? GetExpressionType<T>(functionTokens, sourceParameter, lambdaParameters),
				formatProvider);

			var right = CreateExpression<T>(functionTokens.Right, sourceParameter, lambdaParameters, GetFunctionParameterType(functionTokens.Operation) ?? left.Type, formatProvider);

			return left == null
				? null
				: GetFunction(functionTokens.Operation, left, right, sourceParameter, lambdaParameters);
		}

		/// <summary>
		/// Used to get the ParameterExpressions used in an Expression so that Expression.Call will have the correct number of parameters supplied.
		/// </summary>
		private class ParameterVisitor : ExpressionVisitor
		{
			private static readonly string[] AnyAllMethodNames = new[] { "Any", "All" };
			private List<ParameterExpression> _parameters;

			public IEnumerable<ParameterExpression> GetParameters(Expression expr)
			{
				Contract.Requires(expr != null);
				Contract.Ensures(Contract.Result<IEnumerable<ParameterExpression>>() != null);

				_parameters = new List<ParameterExpression>();
				Visit(expr);
				return _parameters;
			}

			public override Expression Visit(Expression node)
			{
				if (node.NodeType == ExpressionType.Call && AnyAllMethodNames.Contains(((MethodCallExpression)node).Method.Name))
				{
					// Skip the second parameter of the Any/All as this has already been covered
					return base.Visit(((MethodCallExpression)node).Arguments.First());
				}

				return base.Visit(node);
			}

			protected override Expression VisitParameter(ParameterExpression p)
			{
				if (!_parameters.Contains(p))
				{
					_parameters.Add(p);
				}

				return base.VisitParameter(p);
			}
		}
	}
}