// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.IO;

	/// <summary>
	/// Defines the public interface for an async REST client.
	/// </summary>
#if !WINDOWS_PHONE
	[ContractClass(typeof(AsyncRestClientContracts))]
#endif
	public interface IAsyncRestClient
	{
		/// <summary>
		/// Begins the REST request.
		/// </summary>
		/// <param name="callback">The <see cref="AsyncCallback"/> to invoke when response is received.</param>
		/// <param name="state">The asynchronous state object.</param>
		/// <returns>An <see cref="IAsyncResult"/> instance.</returns>
		IAsyncResult BeginGetResult(AsyncCallback callback, object state);

		/// <summary>
		/// Gets the result from the passed <see cref="IAsyncResult"/>.
		/// </summary>
		/// <param name="result">The async operation result.</param>
		/// <returns>The downloaded resource as a <see cref="string"/>.</returns>
		Stream EndGetResult(IAsyncResult result);
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(IAsyncRestClient))]
	internal abstract class AsyncRestClientContracts : IAsyncRestClient
	{
		public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
		{
			Contract.Requires<ArgumentNullException>(callback != null);
			throw new NotImplementedException();
		}

		public Stream EndGetResult(IAsyncResult result)
		{
			Contract.Requires<ArgumentNullException>(result != null);
			Contract.Ensures(Contract.Result<Stream>() != null);
			throw new NotImplementedException();
		}
	}
#endif
}