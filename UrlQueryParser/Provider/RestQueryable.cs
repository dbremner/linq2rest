// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.Script.Serialization;

	public class RestQueryable<T> : IOrderedQueryable<T>
	{
		private readonly IRestClient _client;

		public RestQueryable(IRestClient client, JavaScriptSerializer serializer)
		{
			_client = client;
			Provider = new RestQueryProvider<T>(_client, serializer);
			Expression = Expression.Constant(this);
		}

		public RestQueryable(IRestClient client, JavaScriptSerializer serializer, Expression expression)
			: this(client, serializer)
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

		public IEnumerator<T> GetEnumerator()
		{
			return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
		}
	}
}