namespace UrlQueryParser.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	public class RestQueryable<T> : IQueryable<T>
	{
		public RestQueryable()
		{
			Provider = new RestQueryProvider<T>();
			Expression = Expression.Constant(this);
		}

		public RestQueryable(Expression expression)
			: this()
		{
			Expression = expression;
		}

		/// <summary>
		/// 	type of T in IQueryable of T
		/// </summary>
		public Type ElementType
		{
			get { return typeof(T); }
		}

		/// <summary>
		/// 	expression tree
		/// </summary>
		public Expression Expression { get; private set; }

		/// <summary>
		/// 	IQueryProvider part of LINQ to Twitter
		/// </summary>
		public IQueryProvider Provider { get; private set; }

		public IEnumerator<T> GetEnumerator() { return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
		}
	}
}