// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !SILVERLIGHT
	using System.Diagnostics.Contracts;
#endif
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Linq2Rest.Provider;

	internal class AsyncExpressionProcessor : IAsyncExpressionProcessor
	{
		private readonly IExpressionVisitor _visitor;

		public AsyncExpressionProcessor(IExpressionVisitor visitor)
		{
			_visitor = visitor;
		}

		public Task<IObservable<T>> ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, Task<IList<T>>> resultLoader, Func<Type, ParameterBuilder, Task<IEnumerable>> intermediateResultLoader)
		{
			var task = ProcessMethodCallInternal(methodCall, builder, resultLoader, intermediateResultLoader);
			return task == null
					? resultLoader(builder).ContinueWith(x => x.Result.ToObservable())
					: task.ContinueWith(o => LoadIntermediateResult<T>(o));
		}

		private static IObservable<T> LoadIntermediateResult<T>(Task<object> o)
		{
			if (o.IsCompleted)
			{
				return (IObservable<T>)o.Result;
			}

			if (o.IsFaulted && o.Exception != null)
			{
				throw o.Exception;
			}

			throw new Exception("Could not get observable result.");
		}

		private static Task<object> InvokeEager<T>(MethodCallExpression methodCall, object source)
		{
			var parameters = ResolveInvocationParameters(source as IEnumerable, typeof(T), methodCall);
			return Task.Factory.StartNew(() => methodCall.Method.Invoke(null, parameters));
		}

		private static object[] ResolveInvocationParameters(IEnumerable results, Type type, MethodCallExpression methodCall)
		{
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

		private Task<object> ProcessMethodCallInternal<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, Task<IList<T>>> resultLoader, Func<Type, ParameterBuilder, Task<IEnumerable>> intermediateResultLoader)
		{
#if !SILVERLIGHT
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
#if !SILVERLIGHT
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						var newFilter = _visitor.Visit(methodCall.Arguments[1]);

						builder.FilterParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
													? newFilter
													: string.Format("({0}) and ({1})", builder.FilterParameter, newFilter);
					}

					break;
				case "Select":
#if !SILVERLIGHT
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
#if !SILVERLIGHT
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						builder.TakeParameter = _visitor.Visit(methodCall.Arguments[1]);
					}

					break;
				case "Skip":
#if !SILVERLIGHT
					Contract.Assume(methodCall.Arguments.Count >= 2);
#endif
					{
						var result = ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
						if (result != null)
						{
							return InvokeEager<T>(methodCall, result);
						}

						builder.SkipParameter = _visitor.Visit(methodCall.Arguments[1]);
					}

					break;
				default:
					return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
			}

			return null;
		}

		private Task<object> GetMethodResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, Task<IList<T>>> resultLoader, Func<Type, ParameterBuilder, Task<IEnumerable>> intermediateResultLoader)
		{
#if !SILVERLIGHT
			Contract.Assume(methodCall.Arguments.Count >= 2);
#endif

			ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			var processResult = _visitor.Visit(methodCall.Arguments[1]);
			var currentParameter = string.IsNullOrWhiteSpace(builder.FilterParameter)
									? processResult
									: string.Format("({0}) and ({1})", builder.FilterParameter, processResult);
			builder.FilterParameter = currentParameter;

			var genericArguments = methodCall.Method.GetGenericArguments();
			var method = typeof(Queryable)
				.GetMethods()
				.Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1)
				.MakeGenericMethod(genericArguments);

			return resultLoader(builder)
				.ContinueWith(
							  t =>
							  {
								  var list = t.Result;

#if !SILVERLIGHT
								  Contract.Assume(list != null);
#endif

								  var parameters = new object[] { list.ToObservable().AsQbservable() };

								  return method.Invoke(null, parameters);
							  });
		}

		private Task<object> GetResult<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, Task<IList<T>>> resultLoader, Func<Type, ParameterBuilder, Task<IEnumerable>> intermediateResultLoader)
		{
#if !SILVERLIGHT
			Contract.Assume(methodCall.Arguments.Count >= 1);
#endif

			ProcessMethodCallInternal(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

			return resultLoader(builder)
				.ContinueWith(
							  t =>
							  {
								  var list = t.Result;

#if !SILVERLIGHT
								  Contract.Assume(list != null);
#endif

								  var parameters = ResolveInvocationParameters(list, typeof(T), methodCall);
								  return methodCall.Method.Invoke(null, parameters);
							  });
		}

		private Task<object> ExecuteMethod<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, Task<IList<T>>> resultLoader, Func<Type, ParameterBuilder, Task<IEnumerable>> intermediateResultLoader)
		{
#if !SILVERLIGHT
			Contract.Requires(resultLoader != null);
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

			var genericArgument = innerMethod.Method.ReturnType.GetGenericArguments()[0];
			var type = typeof(T);

			var methodInfo = methodCall.Method;

			var list = type != genericArgument
						? intermediateResultLoader(genericArgument, builder)
							.ContinueWith(
										  t =>
										  {
											  var resultList = t.Result;
											  var arguments = ResolveInvocationParameters(resultList, genericArgument, methodCall);
											  var methodResult = methodInfo.Invoke(null, arguments);

											  return methodResult;
										  })
						: resultLoader(builder)
							.ContinueWith(
										  t =>
										  {
											  var resultList = t.Result;
											  var arguments = ResolveInvocationParameters(resultList, genericArgument, methodCall);
											  return methodInfo.Invoke(null, arguments);
										  });

			return list;
		}
	}
}