using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Linq2Rest.Provider {
    /// <summary>
    /// Extensions to IQueryable
    /// </summary>
    public static class QueryableExtensions {
        /// <summary>
        /// Specifies the navigation properties to eagerly expand on the specified source when fetched.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="paths">The paths int the form "Child1, Child2/GrandChild1.</param>
        /// <returns></returns>
        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, string paths) {

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!(source is RestQueryable<TSource>)) {
                return source;
            }
            return source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(paths) })) as IOrderedQueryable<TSource>;
        }

         /// <summary>
         /// Specifies the navigation properties to eagerly expand on the specified source when fetched.
         /// </summary>
         /// <typeparam name="TSource"></typeparam>
         /// <param name="source">The source.</param>
         /// <param name="paths">The paths int the form "Child1, Child2/GrandChild1.</param>
         /// <returns></returns>
         public static IOrderedQueryable<TSource> Expand<TSource>(this IOrderedQueryable<TSource> source, string paths) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            if (!(source is RestQueryable<TSource>)) {
                return source;
            }
            return source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(paths) })) as IOrderedQueryable<TSource>;
         }
    }
}