// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
#if !SILVERLIGHT
	using System.Diagnostics.Contracts;
#endif

	/// <summary>
	/// Defines the public interface for the async REST client factory.
	/// </summary>
#if !SILVERLIGHT
	[ContractClass(typeof(AsyncRestClientFactoryContracts))]
#endif
	public interface IAsyncRestClientFactory
	{
		/// <summary>
		/// Gets the base service address.
		/// </summary>
		Uri ServiceBase { get; }

		/// <summary>
		/// Creates an <see cref="IAsyncRestClient"/>.
		/// </summary>
		/// <param name="source">The <see cref="Uri"/> to download from.</param>
		/// <returns>An <see cref="IAsyncRestClient"/> instance.</returns>
		IAsyncRestClient Create(Uri source);
	}

#if !SILVERLIGHT
	[ContractClassFor(typeof(IAsyncRestClientFactory))]
	internal abstract class AsyncRestClientFactoryContracts : IAsyncRestClientFactory
	{
		public Uri ServiceBase
		{
			get
			{
				Contract.Ensures(Contract.Result<Uri>() != null);

				throw new NotImplementedException();
			}
		}

		public IAsyncRestClient Create(Uri source)
		{
			Contract.Requires<ArgumentNullException>(source != null);

			throw new NotImplementedException();
		}
	}
#endif
}