// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncRestClient.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the public interface for an async REST client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.IO;
	using System.Threading.Tasks;

	/// <summary>
	/// Defines the public interface for an async REST client.
	/// </summary>
#if !WINDOWS_PHONE
	[ContractClass(typeof(AsyncRestClientContracts))]
#endif
	public interface IAsyncRestClient
	{
		/// <summary>
		/// Gets a service response.
		/// </summary>
		/// <returns>The service response as a <see cref="Task{Stream}"/>.</returns>
		Task<Stream> Download();
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(IAsyncRestClient))]
	internal abstract class AsyncRestClientContracts : IAsyncRestClient
	{
		public Task<Stream> Download()
		{
			Contract.Ensures(Contract.Result<Task<Stream>>() != null);
			throw new NotImplementedException();
		}
	}
#endif
}