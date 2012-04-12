// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Collections.Generic;

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Concurrent;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text.RegularExpressions;

	/// <summary>
	/// Defines the FilterExpressionFactory.
	/// </summary>
	public class FilterExpressionFactory : IFilterExpressionFactory
	{
		private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US");
		private static readonly Regex StringRx = new Regex(@"^[""']([^""']*?)[""']$", RegexOptions.Compiled);
		private static readonly Regex FunctionRx = new Regex(@"^([^\(\)]+)\((.+)\)$", RegexOptions.Compiled);
		private static readonly Regex FunctionContentRx = new Regex(@"^(.*\((?>[^()]+|\((?<Depth>.*)|\)(?<-Depth>.*))*(?(Depth)(?!))\)|.*?)\s*,\s*(.+)$", RegexOptions.Compiled);
        //private static readonly Regex AnyAllFunctionRx = new Regex(@"^(.*)/(any|all)\((.*)\)$");
	    private static readonly Regex AnyAllFunctionRx = new Regex(@"^(([0-9a-zA-Z_]*/)+)(any|all)\((.*)\)$");
		private static readonly Regex NewRx = new Regex(@"^new (?<type>[^\(\)]+)\((?<parameters>.*)\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly ConcurrentDictionary<Type, MethodInfo> ParseMethods = new ConcurrentDictionary<Type, MethodInfo>();

		/// <summary>
		/// Creates a filter expression from its string representation.
		/// </summary>
		/// <param name="filter">The string representation of the filter.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item to filter.</typeparam>
		/// <returns>An <see cref="Expression{TDelegate}"/> if the passed filter is valid, otherwise null.</returns>
		public Expression<Func<T, bool>> Create<T>(string filter)
		{
			return Create<T>(filter, DefaultCulture);
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

			return expression == null ? x => true : Expression.Lambda<Func<T, bool>>(expression, parameter);
		}

        private static TokenSet GetAnyAllFunctionTokens(string filter) {
            Contract.Requires(filter != null);

            var functionMatch = AnyAllFunctionRx.Match(filter);
            if (!functionMatch.Success) {
                return null;
            }

            var functionCollection = functionMatch.Groups[1].Value.Trim('/');
            var functionName = functionMatch.Groups[3].Value;
            var functionContent = functionMatch.Groups[4].Value;

            return new FunctionTokenSet {
                Operation = functionName,
                Left = functionCollection,
                Right = functionContent
            };
        }

		private static TokenSet GetFunctionTokens(string filter)
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

		private static Expression GetPropertyExpression<T>(string propertyToken, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters )
		{
			Contract.Requires(propertyToken != null);

			if (!propertyToken.IsImpliedBoolean())
			{
				var token = propertyToken.GetTokens().FirstOrDefault();
				if (token != null)
				{
                    return GetPropertyExpression<T>(token.Left, parameter, lambdaParameters) ?? GetPropertyExpression<T>(token.Right, parameter, lambdaParameters);
				}
			}

			var parentType = typeof(T);
			Expression propertyExpression = null;

			var propertyChain = propertyToken.Split('/');

            if (propertyChain.Any() && lambdaParameters.Any(p => p.Name == propertyChain.First())) {
                ParameterExpression lambdaParameter = lambdaParameters.First(p => p.Name == propertyChain.First());
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

		private static Type GetExpressionType<T>(TokenSet set, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters )
		{
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

			token = token.ToLowerInvariant();

			if (string.Equals("not", token, StringComparison.OrdinalIgnoreCase))
			{
				return GetRightOperation(token, right);
			}

			Contract.Assume(left != null);

			return GetLeftRightOperation(token, left, right);
		}

		private static Expression GetLeftRightOperation(string token, Expression left, Expression right)
		{
			Contract.Requires(token != null);
			Contract.Requires(left != null);
			Contract.Requires(right != null);

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

			throw new InvalidOperationException("Unsupported operation");
		}

		private static Expression GetRightOperation(string token, Expression right)
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

		private static Expression GetFunction(string function, Expression left, Expression right, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters )
		{
			Contract.Requires(function != null);

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
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleRoundMethod : MethodProvider.DecimalRoundMethod, left);
				case "floor":
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleFloorMethod : MethodProvider.DecimalFloorMethod, left);
				case "ceiling":
					Contract.Assume(left != null);

					return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleCeilingMethod : MethodProvider.DecimalCeilingMethod, left);
                case "any":
                    Contract.Assume(left != null);

			        return Expression.Call(MethodProvider.GetAnyMethod(left.Type), left,
			                               Expression.Lambda(
			                                   typeof (Func<,>).MakeGenericType(
			                                       MethodProvider.GetIEnumerableImpl(left.Type).GetGenericArguments()[0],
			                                       typeof (bool)), right, new ParameterVisitor().GetParameters(right).Where(p => p.Name != sourceParameter.Name)));
                case "all":
                    Contract.Assume(left != null);

                    return Expression.Call(MethodProvider.GetAllMethod(left.Type), left, 
                                            Expression.Lambda(
                                                typeof(Func<,>).MakeGenericType(
                                                    MethodProvider.GetIEnumerableImpl(left.Type).GetGenericArguments()[0], 
                                                    typeof(bool)), right, new ParameterVisitor().GetParameters(right).Where(p => p.Name != sourceParameter.Name)));
				default:
					return null;
			}
		}

        /// <summary>
        /// Used to get the ParameterExpressions used in an Expression so that Expression.Call will have the correct number of parameters supplied
        /// </summary>
        private class ParameterVisitor : ExpressionVisitor {

            List<ParameterExpression> m_Parameters;

            public IEnumerable<ParameterExpression> GetParameters(Expression expr) {
                m_Parameters = new List<ParameterExpression>();
                Visit(expr);
                return m_Parameters;
            }

            public override Expression Visit(Expression node) {
                
                if (node is MethodCallExpression && new[] { "Any", "All" }.Contains(((MethodCallExpression)node).Method.Name)) {
                    // Skip the second parameter of the Any/All as this has already been covered
                    return base.Visit(((MethodCallExpression) node).Arguments.First());
                }
                return base.Visit(node);
            }

            protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression p) {
                
                if (!m_Parameters.Contains(p))
                    m_Parameters.Add(p);

                return base.VisitParameter(p);
            }
        }

		private static Expression GetKnownConstant(string token, Type type, IFormatProvider formatProvider)
		{
			Contract.Requires(token != null);

			if (string.Equals(token, "null", StringComparison.OrdinalIgnoreCase))
			{
				return Expression.Constant(null);
			}

			if (type == typeof(Guid) || type == typeof(Guid?))
			{
				return
					string.Equals(token, "newguid()", StringComparison.OrdinalIgnoreCase)
						? Expression.Convert(Expression.Constant(Guid.NewGuid()), type)
						: string.Equals(token, "empty", StringComparison.OrdinalIgnoreCase)
							? Expression.Convert(Expression.Constant(Guid.Empty), type)
							: Expression.Convert(Expression.Constant(Guid.Parse(token)), type);
			}

			if (type != null)
			{
				var parseMethod = ParseMethods.GetOrAdd(type, ResolveParseMethod);
				if (parseMethod != null)
				{
					var parseResult = parseMethod.Invoke(null, new object[] { token, formatProvider });
					return Expression.Constant(parseResult);
				}

				if (type.IsEnum)
				{
					var enumValue = Enum.Parse(type, token, true);
					return Expression.Constant(enumValue);
				}

				if (type.IsGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
				{
					var genericTypeArgument = type.GetGenericArguments()[0];
					var value = GetKnownConstant(token, genericTypeArgument, formatProvider);
					if (value != null)
					{
						return Expression.Convert(value, type);
					}
				}
			}

			return null;
		}

		private static MethodInfo ResolveParseMethod(Type type)
		{
			Contract.Requires(type != null);

			return type.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.Where(x => x.Name == "Parse" && x.GetParameters().Length == 2)
				.Where(x => x.GetParameters().First().ParameterType == typeof(string) && x.GetParameters().ElementAt(1).ParameterType == typeof(IFormatProvider))
				.FirstOrDefault();
		}

		private Expression CreateExpression<T>(string filter, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider)
		{
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

			if (expression == null)
			{
				expression = GetConstructorExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

			if (expression == null)
			{
				expression = GetArithmeticExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

            if (expression == null) {
                expression = GetAnyAllFunctionExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
            }

			if (expression == null)
			{
				expression = GetFunctionExpression<T>(filter, sourceParameter, lambdaParameters, type, formatProvider);
			}

			if (expression == null)
			{
				expression = GetPropertyExpression<T>(filter, sourceParameter, lambdaParameters);
			}

			if (expression == null)
			{
				expression = GetKnownConstant(filter, type, formatProvider);
			}

			if (expression == null)
			{
				Contract.Assume(type != null);

				expression = Expression.Constant(Convert.ChangeType(filter, type, formatProvider), type);
			}

			return expression;
		}

		private Expression GetTokenExpression<T>(ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider, TokenSet[] tokens)
		{
			Contract.Requires(tokens != null);

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
					var right = CreateExpression<T>(tokenSet.Right, parameter, lambdaParameters, left.Type, formatProvider);

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

						if (resultType == null)
						{
							throw new ArgumentNullException("resultType");
						}

						return Expression.Convert(Expression.New(constructorInfo, parameterExpressions), resultType);
					}
					catch
					{
					}
				}
			}

			return null;
		}

        private Expression GetAnyAllFunctionExpression<T>(string filter, ParameterExpression sourceParameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider) {
            Contract.Requires(filter != null);

            var functionTokens = GetAnyAllFunctionTokens(filter);
            if (functionTokens == null) {
                return null;
            }

            //Type leftType = type ?? GetExpressionType<T>(functionTokens, sourceParameter, lambdaParameters);
            Type leftType = GetPropertyExpression<T>(functionTokens.Left, sourceParameter, lambdaParameters).Type;
            var left = CreateExpression<T>(
                functionTokens.Left,
                sourceParameter,
                lambdaParameters,
                leftType,
                formatProvider);

            // Create a new ParameterExpression from the lambda parameter and add to a collection to pass around
            string parameterName = functionTokens.Right.Substring(0, functionTokens.Right.IndexOf(":")).Trim();
            ParameterExpression lambdaParameter =
                Expression.Parameter(MethodProvider.GetIEnumerableImpl(leftType).GetGenericArguments()[0], parameterName);
            lambdaParameters.Add(lambdaParameter);
            var right = CreateExpression<T>(functionTokens.Right.Substring(functionTokens.Right.IndexOf(":") + 1).Trim(), sourceParameter, lambdaParameters, GetFunctionParameterType(functionTokens.Operation) ?? left.Type, formatProvider);

            return GetFunction(functionTokens.Operation, left, right, sourceParameter, lambdaParameters);
        }

        private Expression GetFunctionExpression<T>(string filter, ParameterExpression parameter, ICollection<ParameterExpression> lambdaParameters, Type type, IFormatProvider formatProvider)
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
                lambdaParameters,
				type ?? GetExpressionType<T>(functionTokens, parameter, lambdaParameters),
				formatProvider);

			var right = CreateExpression<T>(functionTokens.Right, parameter, lambdaParameters, GetFunctionParameterType(functionTokens.Operation) ?? left.Type, formatProvider);

			return GetFunction(functionTokens.Operation, left, right, parameter, lambdaParameters);
		}
	}
}