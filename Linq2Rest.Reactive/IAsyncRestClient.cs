// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncRestClient.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
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
		Task<Stream> Get();

		/// <summary>
		/// Posts the passed data to the service.
		/// </summary>
		/// <param name="input">The <see cref="Stream"/> representation to post.</param>
		/// <returns>The service response as a <see cref="Task{Stream}"/>.</returns>
		Task<Stream> Post(Stream input);

		/// <summary>
		/// Puts the passed data to the service.
		/// </summary>
		/// <param name="input">The <see cref="Stream"/> representation to put.</param>
		/// <returns>The service response as a <see cref="Task{Stream}"/>.</returns>
		Task<Stream> Put(Stream input);

		/// <summary>
		/// Deletes the resource at the service.
		/// </summary>
		/// <returns>The service response as a <see cref="Task{Stream}"/>.</returns>
		Task<Stream> Delete();
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(IAsyncRestClient))]
	internal abstract class AsyncRestClientContracts : IAsyncRestClient
	{
		public Task<Stream> Get()
		{
			Contract.Ensures(Contract.Result<Task<Stream>>() != null);
			throw new NotImplementedException();
		}

		public Task<Stream> Post(Stream input)
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Ensures(Contract.Result<Task<Stream>>() != null);
			throw new NotImplementedException();
		}

		public Task<Stream> Put(Stream input)
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Ensures(Contract.Result<Task<Stream>>() != null);
			throw new NotImplementedException();
		}

		public Task<Stream> Delete()
		{
			Contract.Ensures(Contract.Result<Task<Stream>>() != null);
			throw new NotImplementedException();
		}
	}
#endif
}