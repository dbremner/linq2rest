// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reactive.Linq;
	using System.Reflection;
	using Linq2Rest.Provider;

	internal class AsyncExpressionProcessor : IAsyncExpressionProcessor
	{
		private readonly IExpressionWriter _writer;

		public AsyncExpressionProcessor(IExpressionWriter writer)
		{
#if !WINDOWS_PHONE
			Contract.Requires(writer != null);
#endif
			_writer = writer;
		}

		public IObservable<T> ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
			var task = ProcessMethodCallInternal(methodCall, builder, resultLoader, intermediateResultLoader);
			return task == null
					? (IObservable<T>)resultLoader(builder).SelectMany(x => x)
					: task.Select(o => (IQbservable<T>)o).SelectMany(x => x);
		}

		private static IObservable<object> InvokeEager<T>(MethodCallExpression methodCall, object source)
		{
#if !WINDOWS_PHONE
			Contract.Requires(methodCall != null);
#endif

			var enumerableSource = source as IEnumerable;

			Contract.Assume(enumerableSource != null);

			var parameters = ResolveInvocationParameters(enumerableSource, typeof(T), methodCall);
			return Observable.Return(methodCall.Method.Invoke(null, parameters));
		}

		private static object[] ResolveInvocationParameters(IEnumerable results, Type type, MethodCallExpression methodCall)
		{
#if !WINDOWS_PHONE
			Contract.Requires(results != null);
			Contract.Requires(type != null);
			Contract.Requires(methodCall != null);
#endif
			var parameters = new[] { results.ToQbservable(type) }
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

		private IObservable<object> ProcessMethodCallInternal<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
#if !WINDOWS_PHONE
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
#endif
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
				case "Where":
#if !WINDOWS_PHONE
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						var newFilter = _writer.Write(methodCall.Arguments[1]);

						builder.FilterParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
													? newFilter
													: string.Format("({0}) and ({1})", builder.FilterParameter, newFilter);
					}

					break;
				case "Select":
#if !WINDOWS_PHONE
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
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
				case "Take":
#if !WINDOWS_PHONE
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						builder.TakeParameter = _writer.Write(methodCall.Arguments[1]);
					}

					break;
				case "Skip":
#if !WINDOWS_PHONE
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						builder.SkipParameter = _writer.Write(methodCall.Arguments[1]);
					}

					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
			}

			return null;
		}

		private IObservable<object> GetMethodResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
#if !WINDOWS_PHONE
			Contract.Requires(methodCall != null);
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
			Contract.Assume(methodCall.Arguments.Count >= 2);
#endif

			ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			var processResult = _writer.Write(methodCall.Arguments[1]);
			var currentParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: string.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

			var genericArguments = methodCall.Method.GetGenericArguments();
#if !NETFX_CORE
			var method = typeof(Queryable)
				.GetMethods()
				.Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1)
				.MakeGenericMethod(genericArguments);
#else
			var method = typeof(Queryable).GetTypeInfo()
				.GetDeclaredMethods(methodCall.Method.Name)
				.Single(x => x.GetParameters().Length == 1)
				.MakeGenericMethod(genericArguments);
#endif

			return resultLoader(builder)
				.Select<IEnumerable<T>,object>(
							  list =>
							  {
#if !WINDOWS_PHONE
								  Contract.Assume(list != null);
#endif

								  var qbservable = list.ToObservable().AsQbservable();
								  var parameters = new object[] { qbservable };

								  return method.Invoke(null, parameters);
							  });
		}

		private IObservable<object> GetResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
#if !WINDOWS_PHONE
			Contract.Requires(methodCall != null);
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
			Contract.Assume(methodCall.Arguments.Count >= 1);
#endif

			ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			return resultLoader(builder)
				.Select(
							  list =>
							  {
#if !WINDOWS_PHONE
								  Contract.Assume(list != null);
#endif

								  var parameters = ResolveInvocationParameters(list, typeof(T), methodCall);
								  return methodCall.Method.Invoke(null, parameters);
							  });
		}

		private IObservable<object> ExecuteMethod<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
#if !WINDOWS_PHONE
			Contract.Requires(methodCall != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);
			Contract.Requires(builder != null);
			Contract.Assume(methodCall.Arguments.Count >= 2);
#endif

			var innerMethod = methodCall.Arguments[0] as MethodCallExpression;

			if (innerMethod == null)
			{
				return null;
			}

			var result = ProcessMethodCallInternal(innerMethod, builder, resultLoader, intermediateResultLoader);
			if (result != null)
			{
				return InvokeEager<T>(innerMethod, result);
			}

#if !NETFX_CORE
			var genericArgument = innerMethod.Method.ReturnType.GetGenericArguments()[0];
#else
			var genericArgument = innerMethod.Method.ReturnType.GenericTypeArguments[0];
#endif
			var type = typeof(T);

			var methodInfo = methodCall.Method;

			var observable = type != genericArgument
						? intermediateResultLoader(genericArgument, builder)
							.Select(
										  resultList =>
										  {
											  var arguments = ResolveInvocationParameters(resultList as IEnumerable, genericArgument, methodCall);
											  var methodResult = methodInfo.Invoke(null, arguments);

											  return methodResult;
										  })
						: resultLoader(builder)
							.Select(
										  resultList =>
										  {
											  var arguments = ResolveInvocationParameters(resultList as IEnumerable, genericArgument, methodCall);
											  return methodInfo.Invoke(null, arguments);
										  });

			return observable;
		}

#if !WINDOWS_PHONE
		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_writer != null);
		}
#endif
	}
}