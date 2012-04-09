// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Implementations
{
	using System;
	using System.Net;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines the base REST client implementation.
	/// </summary>
	public class RestClientBase : IRestClient
	{
		private readonly WebClient _client;
		private readonly string _acceptHeader;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientBase"/> class.
		/// </summary>
		/// <param name="uri">The base <see cref="Uri"/> for the REST service.</param>
		/// <param name="acceptHeader">The accept header to use in web requests.</param>
		protected RestClientBase(Uri uri, string acceptHeader)
		{
			_client = new WebClient();
			_acceptHeader = acceptHeader;
			ServiceBase = uri;
		}

		/// <summary>
		/// Gets the base <see cref="Uri"/> for the REST service.
		/// </summary>
		public Uri ServiceBase { get; private set; }

		/// <summary>
		/// Gets a service response.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/> to load the resource from.</param>
		/// <returns>A string representation of the resource.</returns>
		public string Get(Uri uri)
		{
			_client.Headers[HttpRequestHeader.Accept] = _acceptHeader;

			return _client.DownloadString(uri);
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

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">True if disposing managed types.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client.Dispose();
			}
		}
	}
}