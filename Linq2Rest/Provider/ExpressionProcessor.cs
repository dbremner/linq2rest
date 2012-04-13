// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading;

	internal static class ExpressionProcessor
	{
		private static readonly ExpressionType[] CompositeExpressionTypes = new[] { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };

		public static string ProcessExpression(this Expression expression)
		{
			return expression == null ? null : ProcessExpression(expression, expression.Type);
		}

		public object Process(Expression expression)
		{
			return _visitor.Visit(expression);
		}

		public object ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);

			if (methodCall == null)
			{
					pathPrefixes.Add(currentMemberExpression.Member.Name);
					currentMemberExpression = currentMemberExpression.Expression as MemberExpression;
			}
				pathPrefixes.Reverse();
				var prefix = string.Join("/", pathPrefixes);

				if (!IsMemberOfParameter(memberExpression))
				{
					var collapsedExpression = CollapseCapturedOuterVariables(memberExpression);
					if (!(collapsedExpression is MemberExpression))
					{
						Contract.Assume(collapsedExpression != null);

						return ProcessExpression(collapsedExpression);
					}

						var newFilter = _visitor.Visit(methodCall.Arguments[1]);

				var memberCall = GetMemberCall(memberExpression);

				var innerExpression = memberExpression.Expression;

				Contract.Assume(innerExpression != null);

				return string.IsNullOrWhiteSpace(memberCall)
						? prefix
						: string.Format("{0}({1})", memberCall, ProcessExpression(innerExpression));
					}
					break;
				case "Select":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
				var value = (expression as ConstantExpression).Value;

				Contract.Assume(type != null);

				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					"{0}{1}{0}",
					value is string ? "'" : string.Empty,
					value == null ? "null" : GetValue(Expression.Convert(expression, type)));
						}
						var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
						if (unaryExpression != null)
						{
				var unaryExpression = expression as UnaryExpression;
				var operand = unaryExpression.Operand;
				switch (unaryExpression.NodeType)
							{
					case ExpressionType.Not:
					case ExpressionType.IsFalse:
						return string.Format("not({0})", ProcessExpression(operand));
					default:
						return ProcessExpression(operand);
				}
			}

			if (expression is BinaryExpression)
								{
				var binaryExpression = expression as BinaryExpression;
				var operation = GetOperation(binaryExpression);

				var isLeftComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Left.NodeType);
				var isRightComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Right.NodeType);

				var leftType = GetUnconvertedType(binaryExpression.Left);
				var leftString = ProcessExpression(binaryExpression.Left);
				var rightString = ProcessExpression(binaryExpression.Right, leftType);

				return string.Format(
					"{0} {1} {2}",
					string.Format(isLeftComposite ? "({0})" : "{0}", leftString),
					operation,
					string.Format(isRightComposite ? "({0})" : "{0}", rightString));
			}

			if (expression is MethodCallExpression)
									{
				return GetMethodCall(expression as MethodCallExpression);
									}

									builder.SelectParameter = String.Join(",", args);
								}

			throw new InvalidOperationException("Expression is not recognized or supported");
							}

		private static Type GetUnconvertedType(Expression expression)
		{
			Contract.Requires(expression != null);

			switch (expression.NodeType)
			{
				case ExpressionType.Convert:
					var unaryExpression = expression as UnaryExpression;

					Contract.Assume(unaryExpression != null, "Matches node type.");

					return unaryExpression.Operand.Type;
				default:
					return expression.Type;
						}
					}
					break;
				case "OrderBy":
				case "ThenBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
				if (name == "Length")
						{
					return name.ToLowerInvariant();
						}
						var item = _visitor.Visit(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(item);
					}
					break;
				case "OrderByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
				switch (name)
						{
					case "Hour":
					case "Minute":
					case "Second":
					case "Day":
					case "Month":
					case "Year":
						return name.ToLowerInvariant();
				}
						}
						var visit = _visitor.Visit(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(visit + " desc");
					}
					break;
				case "ThenBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
			if (input == null || input.NodeType != ExpressionType.MemberAccess)
						{
				return input;
						}
						var visit = _visitor.Visit(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(visit);
					}
					break;
				case "ThenByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
				var obj = constantExpression.Value;
				if (obj == null)
						{
					return input;
						}
						var visit = _visitor.Visit(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(visit + " desc");
					}
					break;
				case "Take":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
					var result = propertyInfo.GetValue(obj, null);
					return result is Expression ? (Expression)result : Expression.Constant(result);
				}
			}

			return input;
		}

		private static object GetValue(Expression input)
						{
			Contract.Requires(input != null);

			var objectMember = Expression.Convert(input, typeof(object));
			var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

			return getterLambda();
						}
						builder.TakeParameter = _visitor.Visit(methodCall.Arguments[1]);
					}
					break;
				case "Skip":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.SkipParameter = _visitor.Visit(methodCall.Arguments[1]);
					}
					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
			}

		private static string GetMethodCall(MethodCallExpression expression)
		{
			Contract.Requires(expression != null);

			var methodName = expression.Method.Name;
			var declaringType = expression.Method.DeclaringType;
			if (declaringType == typeof(string))
			{
				var obj = expression.Object;

				Contract.Assume(obj != null);

				switch (methodName)
				{
					case "Replace":
						{
							Contract.Assume(expression.Arguments.Count > 1);

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];

							Contract.Assume(firstArgument != null);
							Contract.Assume(secondArgument != null);

							return string.Format(
								"replace({0}, {1}, {2})",
								ProcessExpression(obj),
								ProcessExpression(firstArgument),
								ProcessExpression(secondArgument));
		}

					case "Trim":
						return string.Format("trim({0})", ProcessExpression(obj));
					case "ToLower":
					case "ToLowerInvariant":
						return string.Format("tolower({0})", ProcessExpression(obj));
					case "ToUpper":
					case "ToUpperInvariant":
						return string.Format("toupper({0})", ProcessExpression(obj));
					case "Substring":
		{
							Contract.Assume(expression.Arguments.Count > 0);

							if (expression.Arguments.Count == 1)
							{
								var argumentExpression = expression.Arguments[0];

			var processResult = _visitor.Visit(methodCall.Arguments[1]);
			var currentParameter = String.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: String.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

								return string.Format(
									"substring({0}, {1})", ProcessExpression(obj), ProcessExpression(argumentExpression));
							}

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];

							Contract.Assume(firstArgument != null);
							Contract.Assume(secondArgument != null);

							return string.Format(
								"substring({0}, {1}, {2})",
								ProcessExpression(obj),
								ProcessExpression(firstArgument),
								ProcessExpression(secondArgument));
		}

					case "IndexOf":
		{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("indexof({0}, {1})", ProcessExpression(obj), ProcessExpression(argumentExpression));
		}

					case "EndsWith":
		{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("endswith({0}, {1})", ProcessExpression(obj), ProcessExpression(argumentExpression));
			}

					case "StartsWith":
			{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("startswith({0}, {1})", ProcessExpression(obj), ProcessExpression(argumentExpression));
						}
				}
			}
			else if (declaringType == typeof(Math))
			{
				Contract.Assume(expression.Arguments.Count > 0);

				var mathArgument = expression.Arguments[0];

				Contract.Assume(mathArgument != null);

				switch (methodName)
				{
					case "Round":
						return string.Format("round({0})", ProcessExpression(mathArgument));
					case "Floor":
						return string.Format("floor({0})", ProcessExpression(mathArgument));
					case "Ceiling":
						return string.Format("ceiling({0})", ProcessExpression(mathArgument));
				}
		}

		private static object InvokeEager(MethodCallExpression methodCall, object source)
		{
			var parameters = ResolveInvocationParameters(source as IEnumerable, methodCall);
			return methodCall.Method.Invoke(null, parameters);
		}

		private static object[] ResolveInvocationParameters(IEnumerable results, MethodCallExpression methodCall)
		{
			var parameters = new object[] { results.AsQueryable() }
				.Concat(methodCall.Arguments.Where((x, i) => i > 0).Select(GetExpressionValue))
				.Where(x => x != null)
				.ToArray();
			return parameters;
		}

		private static object GetExpressionValue(Expression expression)
		{
			if (expression is UnaryExpression)
			{
				return (expression as UnaryExpression).Operand;
			}

			if (expression is ConstantExpression)
			{
				return (expression as ConstantExpression).Value;
			}

			return null;
		}
	}
}