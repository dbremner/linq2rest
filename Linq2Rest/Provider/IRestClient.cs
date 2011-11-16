// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;

	[ContractClass(typeof(RestClientContracts))]
	public interface IRestClient
	{
		Uri ServiceBase { get; }

		string Get(Uri uri);
	}

	[ContractClassFor(typeof(IRestClient))]
	public abstract class RestClientContracts : IRestClient
	{
		public Uri ServiceBase
		{
			get
			{
				Contract.Ensures(Contract.Result<Uri>() != null);
				throw new NotImplementedException();
			}
		}

		public string Get(Uri uri)
		{
			Contract.Requires<ArgumentNullException>(uri != null);

			throw new NotImplementedException();
		}
	}
}