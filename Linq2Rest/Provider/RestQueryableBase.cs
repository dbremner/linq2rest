// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestQueryableBase.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   	This source is subject to the Microsoft Public License (Ms-PL).
//   	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   	All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestQueryableBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	internal class RestQueryableBase<T> : IOrderedQueryable<T>, IDisposable
	{
		public RestQueryableBase(IRestClient client, ISerializerFactory serializerFactory)
		{
			Contract.Requires<ArgumentException>(client != null);
			Contract.Requires<ArgumentException>(serializerFactory != null);

			Client = client;
			SerializerFactory = serializerFactory;
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
		public Expression Expression { get; protected set; }

		/// <summary>
		/// 	IQueryProvider part of RestQueryable.
		/// </summary>
		public IQueryProvider Provider { get; protected set; }

		internal IRestClient Client { get; private set; }

		internal ISerializerFactory SerializerFactory { get; set; }

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
				Client.Dispose();
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(Client != null);
			Contract.Invariant(Expression != null);
		}
	}
}