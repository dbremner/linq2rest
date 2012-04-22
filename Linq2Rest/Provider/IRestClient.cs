// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;

	/// <summary>
	/// Defines the public interface for a REST client.
	/// </summary>
	[ContractClass(typeof(RestClientContracts))]
	public interface IRestClient : IDisposable
	{
		/// <summary>
		/// Gets the base <see cref="Uri"/> for the REST service.
		/// </summary>
		Uri ServiceBase { get; }

		/// <summary>
		/// Gets a service response.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/> to load the resource from.</param>
		/// <returns>A string representation of the resource.</returns>
		Stream Get(Uri uri);
	}

	[ContractClassFor(typeof(IRestClient))]
	internal abstract class RestClientContracts : IRestClient
	{
		public Uri ServiceBase
		{
			get
			{
				Contract.Ensures(Contract.Result<Uri>() != null);
				throw new NotImplementedException();
			}
		}

		public Stream Get(Uri uri)
		{
			Contract.Requires<ArgumentNullException>(uri != null);
			Contract.Ensures(Contract.Result<Stream>() != null);

			throw new NotImplementedException();
		}

		public abstract void Dispose();
	}
}