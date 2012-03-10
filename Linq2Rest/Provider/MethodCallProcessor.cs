// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	internal static class MethodCallProcessor
	{
		public static object ProcessMethodCall<T>(this MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader)
		{
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);

			if (methodCall == null)
			{
				return null;
			}

			var method = methodCall.Method.Name;

			switch (method)
			{
				case "Single":
				case "SingleOrDefault":
					return methodCall.Arguments.Count >= 2
									? GetMethodResult(methodCall, builder, resultLoader)
									: GetResult(methodCall, builder, resultLoader);
				case "First":
				case "FirstOrDefault":
					builder.TakeParameter = "1";
					return methodCall.Arguments.Count >= 2
								? GetMethodResult(methodCall, builder, resultLoader)
								: GetResult(methodCall, builder, resultLoader);
				case "Last":
				case "LastOrDefault":
				case "Count":
				case "LongCount":
					return methodCall.Arguments.Count >= 2
							? GetMethodResult(methodCall, builder, resultLoader)
							: GetResult(methodCall, builder, resultLoader);
				case "Where":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						var newFilter = methodCall.Arguments[1].ProcessExpression();

						builder.FilterParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
													? newFilter
													: string.Format("({0}) and ({1})", builder.FilterParameter, newFilter);
					}
					break;
				case "Select":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
						if (unaryExpression != null)
						{
							var lambdaExpression = unaryExpression.Operand as LambdaExpression;
							if (lambdaExpression != null)
							{
								var selectFunction = lambdaExpression.Body as NewExpression;

								if (selectFunction != null)
								{
									var members = selectFunction.Members.Select(x => x.Name).ToArray();
									var args = selectFunction.Arguments.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();
									if (members.Intersect(args).Count() != members.Length)
									{
										throw new InvalidOperationException("Projection into new member names is not supported.");
									}

									builder.SelectParameter = String.Join(",", args);
								}
							}
						}
					}
					break;
				case "OrderBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.OrderByParameter.Add(methodCall.Arguments[1].ProcessExpression());
					}
					break;
				case "OrderByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.OrderByParameter.Add(methodCall.Arguments[1].ProcessExpression() + " desc");
					}
					break;
				case "ThenBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.OrderByParameter.Add(methodCall.Arguments[1].ProcessExpression());
					}
					break;
				case "ThenByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.OrderByParameter.Add(methodCall.Arguments[1].ProcessExpression() + " desc");
					}
					break;
				case "Take":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.TakeParameter = methodCall.Arguments[1].ProcessExpression();
					}
					break;
				case "Skip":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}
						builder.SkipParameter = methodCall.Arguments[1].ProcessExpression();
					}
					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader);
			}

			return null;
		}

		private static object GetMethodResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader)
		{
			Contract.Assume(methodCall.Arguments.Count >= 2);

			ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);

			var processResult = methodCall.Arguments[1].ProcessExpression();
			var currentParameter = String.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: String.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

			var genericArguments = methodCall.Method.GetGenericArguments();
			var method = typeof(Queryable)
				.GetMethods()
				.Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1)
				.MakeGenericMethod(genericArguments);

			var list = resultLoader(builder);

			Contract.Assume(list != null);

			var parameters = new object[] { list.AsQueryable() };
			return method.Invoke(null, parameters);
		}

		private static object GetResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader)
		{
			Contract.Assume(methodCall.Arguments.Count >= 1);

			ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader);
			var results = resultLoader(builder);

			Contract.Assume(results != null);

			var parameters = ResolveInvocationParameters(results, methodCall);

			var final = methodCall.Method.Invoke(null, parameters);
			return final;
		}

		private static object ExecuteMethod<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader)
		{
			Contract.Assume(methodCall.Arguments.Count >= 2);

			var innerMethod = methodCall.Arguments[0] as MethodCallExpression;

			if (innerMethod == null)
			{
				return null;
			}

			var result = ProcessMethodCall(innerMethod, builder, resultLoader);
			if (result != null)
			{
				return InvokeEager(innerMethod, result);
			}

			var genericArgument = innerMethod.Method.ReturnType.GetGenericArguments()[0];
			var t = typeof(T);
			if (t == genericArgument)
			{

			}

			var list = resultLoader(builder);

			Contract.Assume(list != null);

			var arguments = ResolveInvocationParameters(list, methodCall);

			return methodCall.Method.Invoke(null, arguments);
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