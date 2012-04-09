// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Linq.Expressions;
using System.Reflection;

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
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
        /// <returns></returns>
        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, string paths) {

            if (source == null) {
                throw new ArgumentNullException("source");
            }
            if (!(source is RestQueryable<TSource>)) {
                return source;
            }
            return source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }), new[] { source.Expression, Expression.Constant(paths) }));
        }

        /// <summary>
        /// Expands the specified source.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="paths">The paths to expand in the format "Child1, Child2/GrandChild2".</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> Expand<TSource>(this IOrderedQueryable<TSource> source, string paths) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            if (!(source is RestQueryable<TSource>)) {
                return source;
            }
            return source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }), new[] { source.Expression, Expression.Constant(paths) })) as IOrderedQueryable<TSource>;
        }
	}
}