// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionProcessor.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ExpressionProcessor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	internal class ExpressionProcessor : IExpressionProcessor
	{
		private readonly IExpressionWriter _writer;

		public ExpressionProcessor(IExpressionWriter writer)
		{
			Contract.Requires(writer != null);
			_writer = writer;
		}

		public object ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IEnumerable<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			if (methodCall == null)
			{
				return null;
			}

			var method = methodCall.Method.Name;

			switch (method)
			{
				case "First":
				case "FirstOrDefault":
					builder.TakeParameter = "1";
					return methodCall.Arguments.Count >= 2
								? GetMethodResult(methodCall, builder, resultLoader, intermediateResultLoader)
								: GetResult(methodCall, builder, resultLoader, intermediateResultLoader);

				case "Single":
				case "SingleOrDefault":
				case "Last":
				case "LastOrDefault":
				case "Count":
				case "LongCount":
					return methodCall.Arguments.Count >= 2
							? GetMethodResult(methodCall, builder, resultLoader, intermediateResultLoader)
							: GetResult(methodCall, builder, resultLoader, intermediateResultLoader);
				case "Where":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						var newFilter = _writer.Write(methodCall.Arguments[1]);

						builder.FilterParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
													? newFilter
													: string.Format("({0}) and ({1})", builder.FilterParameter, newFilter);
					}

					break;
				case "Select":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						if (!string.IsNullOrWhiteSpace(builder.SelectParameter))
						{
							return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
						}

						var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
						if (unaryExpression != null)
						{
							var lambdaExpression = unaryExpression.Operand as LambdaExpression;
							if (lambdaExpression != null)
							{
								return ResolveProjection(builder, lambdaExpression);
							}
						}
					}

					break;
				case "OrderBy":
				case "ThenBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						var item = _writer.Write(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(item);
					}

					break;
				case "OrderByDescending":
				case "ThenByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						var visit = _writer.Write(methodCall.Arguments[1]);
						builder.OrderByParameter.Add(visit + " desc");
					}

					break;
				case "Take":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						builder.TakeParameter = _writer.Write(methodCall.Arguments[1]);
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

						builder.SkipParameter = _writer.Write(methodCall.Arguments[1]);
					}

					break;
				case "Expand":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						var expression = methodCall.Arguments[1];

						Contract.Assume(expression != null);

						var objectMember = Expression.Convert(expression, typeof(object));
						var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

						builder.ExpandParameter = getterLambda().ToString();
					}

					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
			}

			return null;
		}

		private static object ResolveProjection(ParameterBuilder builder, LambdaExpression lambdaExpression)
		{
			Contract.Requires(lambdaExpression != null);

			var selectFunction = lambdaExpression.Body as NewExpression;

			if (selectFunction != null)
			{
				var members = selectFunction.Members.Select(x => x.Name)
											.ToArray();
				var args = selectFunction.Arguments.OfType<MemberExpression>()
										 .Select(x => x.Member.Name)
										 .ToArray();
				if (members.Intersect(args).Count() != members.Length)
				{
					throw new InvalidOperationException("Projection into new member names is not supported.");
				}

				builder.SelectParameter = string.Join(",", args);
			}

			var propertyExpression = lambdaExpression.Body as MemberExpression;
			if (propertyExpression != null)
			{
				builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
					? propertyExpression.Member.Name
					: builder.SelectParameter + "," + propertyExpression.Member.Name;
			}

			return null;
		}

		private static object InvokeEager(MethodCallExpression methodCall, object source)
		{
			Contract.Requires(source != null);
			Contract.Requires(methodCall != null);

			var results = source as IEnumerable;

			Contract.Assume(results != null);

			var parameters = ResolveInvocationParameters(results, methodCall);
			return methodCall.Method.Invoke(null, parameters);
		}

		private static object[] ResolveInvocationParameters(IEnumerable results, MethodCallExpression methodCall)
		{
			Contract.Requires(results != null);
			Contract.Requires(methodCall != null);

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

		private object GetMethodResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IEnumerable<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			Contract.Requires(methodCall != null);
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);
			Contract.Assume(methodCall.Arguments.Count >= 2);

			ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			var processResult = _writer.Write(methodCall.Arguments[1]);
			var currentParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: string.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

			var genericArguments = methodCall.Method.GetGenericArguments();
			var queryableMethods = typeof(Queryable).GetMethods();

			Contract.Assume(queryableMethods.Any());

			var nonGenericMethod = queryableMethods
				.Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1);

			Contract.Assume(nonGenericMethod != null);

			var method = nonGenericMethod
				.MakeGenericMethod(genericArguments);

			var list = resultLoader(builder);

			Contract.Assume(list != null);

			var queryable = list.AsQueryable();
			var parameters = new object[] { queryable };
			var result = method.Invoke(null, parameters);
			return result ?? default(T);
		}

		private object GetResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IEnumerable<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			Contract.Requires(builder != null);
			Contract.Requires(methodCall != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);

			Contract.Assume(methodCall.Arguments.Count >= 1);

			ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
			var results = resultLoader(builder);

			Contract.Assume(results != null);

			var parameters = ResolveInvocationParameters(results, methodCall);
			var final = methodCall.Method.Invoke(null, parameters);
			return final;
		}

		private object ExecuteMethod<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IEnumerable<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			Contract.Requires(methodCall != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);
			Contract.Requires(builder != null);
			Contract.Assume(methodCall.Arguments.Count >= 2);

			var innerMethod = methodCall.Arguments[0] as MethodCallExpression;

			if (innerMethod == null)
			{
				return null;
			}

			var result = ProcessMethodCall(innerMethod, builder, resultLoader, intermediateResultLoader);
			if (result != null)
			{
				return InvokeEager(innerMethod, result);
			}

			var genericArgument = innerMethod.Method.ReturnType.GetGenericArguments()[0];
			var type = typeof(T);
			var list = type != genericArgument
			 ? intermediateResultLoader(genericArgument, builder)
			 : resultLoader(builder);

			Contract.Assume(list != null);

			var arguments = ResolveInvocationParameters(list, methodCall);

			return methodCall.Method.Invoke(null, arguments);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_writer != null);
		}
	}
}