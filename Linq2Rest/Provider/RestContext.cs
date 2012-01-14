// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;

	public class RestContext<T>
	{
		private readonly RestQueryable<T> _queryable;

		public RestContext(IRestClient client, ISerializerFactory serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_queryable = new RestQueryable<T>(client, serializerFactory);
		}

		public IQueryable<T> Query
		{
			get
			{
				Contract.Ensures(Contract.Result<IQueryable<T>>() != null);

				return _queryable;
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_queryable != null);
		}
	}
}