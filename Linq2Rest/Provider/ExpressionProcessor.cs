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

	internal class ExpressionProcessor : IExpressionProcessor
	{
		private readonly IExpressionVisitor _visitor;

		public ExpressionProcessor(IExpressionVisitor visitor)
		{
			Contract.Requires(visitor != null);
			_visitor = visitor;
		}

		public object ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
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

						var newFilter = _visitor.Visit(methodCall.Arguments[1]);

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

									builder.SelectParameter = string.Join(",", args);
								}

								var propertyExpression = lambdaExpression.Body as MemberExpression;
								if (propertyExpression != null)
								{
									builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
										? propertyExpression.Member.Name
										: builder.SelectParameter + "," + propertyExpression.Member.Name;
								}
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

						var item = _visitor.Visit(methodCall.Arguments[1]);
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

						var visit = _visitor.Visit(methodCall.Arguments[1]);
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
				case "Expand":
					Contract.Assume(methodCall.Arguments.Count >= 2);
					{
						var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager(methodCall, result);
						}

						var objectMember = Expression.Convert(methodCall.Arguments[1], typeof(object));
						var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

						builder.ExpandParameter = getterLambda().ToString();
					}

					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
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

		private object GetMethodResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
		{
			Contract.Requires(methodCall != null);
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);
			Contract.Assume(methodCall.Arguments.Count >= 2);

			ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			var processResult = _visitor.Visit(methodCall.Arguments[1]);
			var currentParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: string.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

			var genericArguments = methodCall.Method.GetGenericArguments();
			var nonGenericMethod = typeof(Queryable).GetMethods().Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1);

			Contract.Assume(nonGenericMethod != null);

			var method = nonGenericMethod
				.MakeGenericMethod(genericArguments);

			var list = resultLoader(builder);

			Contract.Assume(list != null);

			var parameters = new object[] { list.AsQueryable() };
			return method.Invoke(null, parameters);
		}

		private object GetResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
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

		private object ExecuteMethod<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IList<T>> resultLoader, Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader)
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

			var arguments = ResolveInvocationParameters(list, methodCall);

			return methodCall.Method.Invoke(null, arguments);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_visitor != null);
		}
	}
}