// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines extension methods on IQueryables.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;

	/// <summary>
	/// Defines extension methods on IQueryables.
	/// </summary>
	public static class QueryableExtensions
	{
		/// <summary>
		/// Creates a task to execute the query.
		/// </summary>
		/// <param name="queryable">The <see cref="IQueryable{T}"/> to execute.</param>
		/// <typeparam name="T">The generic type parameter.</typeparam>
		/// <returns>A task returning the query result.</returns>
		public static Task<IEnumerable<T>> ExecuteAsync<T>(this IQueryable<T> queryable)
		{
#if !WINDOWS_PHONE
			Contract.Requires<ArgumentNullException>(queryable != null);
#endif

			return Task.Factory.StartNew(() => queryable.ToArray().AsEnumerable());
		}

		/// <summary>
		/// Expands the specified source.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="paths">The paths to expand in the format "Child1, Child2/GrandChild2".</param>
		/// <returns>An <see cref="IQueryable{T}"/> for continued querying.</returns>
		public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, string paths)
		{
#if !WINDOWS_PHONE
			Contract.Requires<ArgumentNullException>(source != null);
#endif

			if (!(source is RestGetQueryable<TSource>))
			{
				return source;
			}

			return source.Provider.CreateQuery<TSource>(
					Expression.Call(
						null,
						((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }),
						new[] { source.Expression, Expression.Constant(paths) }));
		}

		/// <summary>
		/// Expands the specified source.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="properties">The paths to expand.</param>
		/// <returns>An <see cref="IQueryable{T}"/> for continued querying.</returns>
		public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, params Expression<Func<TSource, object>>[] properties)
		{
#if !WINDOWS_PHONE
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Assume(properties != null);
#endif
			var propertyNames = string.Join(",", properties.Where(x => x != null).Select(ResolvePropertyName));

			return Expand(source, propertyNames);
		}

		/// <summary>
		/// Creates a queryable source where the passed input will be posted to the REST service to create the result.
		/// </summary>
		/// <param name="source">The source <see cref="IQueryable{T}"/></param>
		/// <param name="input">The data to post to the server.</param>
		/// <typeparam name="TResult">The response <see cref="Type"/>.</typeparam>
		/// <typeparam name="TInput">The <see cref="Type"/> of the input data.</typeparam>
		/// <returns></returns>
		public static IQueryable<TResult> Post<TResult, TInput>(this IQueryable<TResult> source, TInput input)
		{
			var restQueryable = source as RestQueryableBase<TResult>;
			if (restQueryable != null)
			{
				var serializer = restQueryable.SerializerFactory.Create<TInput>();
				var stream = serializer.Serialize(input);
				return new RestPostQueryable<TResult>(restQueryable.Client, restQueryable.SerializerFactory, source.Expression, stream);
			}

			return source;
		}
		
		/// <summary>
		/// Creates a queryable source where the passed input will be put to the REST service to create the result.
		/// </summary>
		/// <param name="source">The source <see cref="IQueryable{T}"/></param>
		/// <param name="input">The data to put to the server.</param>
		/// <typeparam name="TResult">The response <see cref="Type"/>.</typeparam>
		/// <typeparam name="TInput">The <see cref="Type"/> of the input data.</typeparam>
		/// <returns></returns>
		public static IQueryable<TResult> Put<TResult, TInput>(this IQueryable<TResult> source, TInput input)
		{
			var restQueryable = source as RestQueryableBase<TResult>;
			if (restQueryable != null)
			{
				var serializer = restQueryable.SerializerFactory.Create<TInput>();
				var stream = serializer.Serialize(input);
				return new RestPutQueryable<TResult>(restQueryable.Client, restQueryable.SerializerFactory, source.Expression, stream);
			}

			return source;
		}
		
		/// <summary>
		/// Creates a queryable source where the passed input will be put to the REST service to create the result.
		/// </summary>
		/// <param name="source">The source <see cref="IQueryable{T}"/></param>
		/// <typeparam name="TResult">The response <see cref="Type"/>.</typeparam>
		/// <returns></returns>
		public static IQueryable<TResult> Delete<TResult>(this IQueryable<TResult> source)
		{
			var restQueryable = source as RestQueryableBase<TResult>;
			if (restQueryable != null)
			{
				return new RestDeleteQueryable<TResult>(restQueryable.Client, restQueryable.SerializerFactory, source.Expression);
			}

			return source;
		}
		
		private static string ResolvePropertyName<TSource>(Expression<Func<TSource, object>> property)
		{
			Contract.Requires(property != null);

			var pathPrefixes = new List<string>();

			var body = property.Body;
			if (body.NodeType == ExpressionType.Convert)
			{
				body = ((UnaryExpression)body).Operand;
			}

			var currentMemberExpression = body as MemberExpression;
			while (currentMemberExpression != null)
			{
				pathPrefixes.Add(currentMemberExpression.Member.Name);
				currentMemberExpression = currentMemberExpression.Expression as MemberExpression;
			}

			pathPrefixes.Reverse();
			return string.Join("/", pathPrefixes);
		}
	}
}