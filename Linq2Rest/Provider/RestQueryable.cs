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

	internal class RestQueryable<T> : IOrderedQueryable<T>, IDisposable
	{
		private readonly IRestClient _client;
		private readonly RestQueryProvider<T> _restQueryProvider;

		public RestQueryable(IRestClient client, ISerializerFactory serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_client = client;
			_restQueryProvider = new RestQueryProvider<T>(_client, serializerFactory);
			Provider = _restQueryProvider;
			Expression = Expression.Constant(this);
		}

		public RestQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression)
			: this(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expression != null);

			Expression = expression;
		}

		/// <summary>
		/// 	<see cref="Type"/> of T in IQueryable of T.
		/// </summary>
		public Type ElementType
		{
			get { return typeof(T); }
		}

		/// <summary>
		/// 	The expression tree.
		/// </summary>
		public Expression Expression { get; private set; }

		/// <summary>
		/// 	IQueryProvider part of RestQueryable.
		/// </summary>
		public IQueryProvider Provider { get; private set; }

		public IEnumerator<T> GetEnumerator()
		{
			return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client.Dispose();
				_restQueryProvider.Dispose();
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_client != null);
			Contract.Invariant(Expression != null);
		}
	}
}