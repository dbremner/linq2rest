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
			Contract.Requires<ArgumentNullException>(queryable != null);

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

			if (!(source is RestQueryable<TSource>))
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
			var propertyNames = string.Join(",", properties.Select(ResolvePropertyName));

			return Expand(source, propertyNames);
		}

		private static string ResolvePropertyName<TSource>(Expression<Func<TSource, object>> property)
		{
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