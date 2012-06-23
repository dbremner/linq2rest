// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE && !NETFX_CORE
	using System.Diagnostics.Contracts;
#endif

    /// <summary>
	/// Defines the public interface for the async REST client factory.
    /// </summary>
#if !WINDOWS_PHONE && !NETFX_CORE
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

#if !WINDOWS_PHONE && !NETFX_CORE
	[ContractClassFor(typeof(IAsyncRestClientFactory))]
	internal abstract class AsyncRestClientFactoryContracts : IAsyncRestClientFactory
	{
		public Uri ServiceBase
		{
			get
			{
				Contract.Ensures(Contract.Result<Uri>() != null);
				Contract.Ensures(Contract.Result<Uri>().Scheme == Uri.UriSchemeHttp || Contract.Result<Uri>().Scheme == Uri.UriSchemeHttps);

				throw new NotImplementedException();
			}
		}

		public IAsyncRestClient Create(Uri source)
		{
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentException>(source.Scheme == Uri.UriSchemeHttp || source.Scheme == Uri.UriSchemeHttps);

			throw new NotImplementedException();
		}
	}
#endif
}