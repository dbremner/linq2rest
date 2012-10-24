// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestContext.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestContext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;

	/// <summary>
	/// Defines the RestContext.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object to query.</typeparam>
	public class RestContext<T> : IDisposable
	{
		private readonly RestGetQueryable<T> _getQueryable;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestContext{T}"/> class.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="serializerFactory"></param>
		public RestContext(IRestClient client, ISerializerFactory serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_getQueryable = new RestGetQueryable<T>(client, serializerFactory);
		}

		/// <summary>
		/// Gets the context query.
		/// </summary>
		public IQueryable<T> Query
		{
			get
			{
				Contract.Ensures(Contract.Result<IQueryable<T>>() != null);

				return _getQueryable;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_getQueryable.Dispose();
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_getQueryable != null);
		}
	}
}