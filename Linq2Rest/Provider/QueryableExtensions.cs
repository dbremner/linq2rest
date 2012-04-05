namespace Linq2Rest.Provider
{
	using System.Collections.Generic;
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
			return Task.Factory.StartNew(() => queryable.ToArray().AsEnumerable());
		}
	}
}