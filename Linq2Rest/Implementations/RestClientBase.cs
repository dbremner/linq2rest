// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Implementations
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Net;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines the base REST client implementation.
	/// </summary>
	public class RestClientBase : IRestClient
	{
		private const string PostMethod = "POST";
		private const string GetMethod = "GET";
		private const string PutMethod = "PUT";
		private const string DeleteMethod = "DELETE";
		private readonly string _acceptHeader;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientBase"/> class.
		/// </summary>
		/// <param name="uri">The base <see cref="Uri"/> for the REST service.</param>
		/// <param name="acceptHeader">The accept header to use in web requests.</param>
		protected RestClientBase(Uri uri, string acceptHeader)
		{
			Contract.Requires<ArgumentNullException>(uri != null);
			Contract.Requires<ArgumentException>(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
			Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(acceptHeader));

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
		public Stream Get(Uri uri)
		{
			var stream = this.GetResponseStream(uri, GetMethod, null);

			Contract.Assume(stream != null);

			return stream;
		}

		/// <summary>
		/// Posts the passed data to the service.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/> to load the resource from.</param>
		/// <param name="input">The <see cref="Stream"/> representation to post.</param>
		/// <returns>The service response as a <see cref="Stream"/>.</returns>
		public Stream Post(Uri uri, Stream input)
		{
			var stream = GetResponseStream(uri, PostMethod, input);

			Contract.Assume(stream != null);

			return stream;
		}

		/// <summary>
		/// Puts the passed data to the service.
		/// </summary>
		/// <param name="input">The <see cref="Stream"/> representation to put.</param>
		/// <param name="uri">The <see cref="Uri"/> to load the resource from.</param>
		/// <returns>The service response as a <see cref="Stream"/>.</returns>
		public Stream Put(Uri uri, Stream input)
		{
			var stream = GetResponseStream(uri, PutMethod, input);

			Contract.Assume(stream != null);

			return stream;
		}

		/// <summary>
		/// Deletes the resource at the service.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/> to load the resource from.</param>
		/// <returns>The service response as a <see cref="Stream"/>.</returns>
		public Stream Delete(Uri uri)
		{
			var stream = GetResponseStream(uri, DeleteMethod, null);

			Contract.Assume(stream != null);

			return stream;
		}

		private Stream GetResponseStream(Uri uri, string method, Stream inputStream)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = method;
			if (method == PostMethod || method == PutMethod)
			{
				request.ContentType = _acceptHeader;
			}

			if (inputStream != null)
			{
				var requestStream = request.GetRequestStream();
				int nextByte;
				while ((nextByte = inputStream.ReadByte()) > -1)
				{
					requestStream.WriteByte((byte)nextByte);
				}
				requestStream.Flush();
			}

			request.Accept = _acceptHeader;
			var response = request.GetResponse();
			var stream = response.GetResponseStream();
			return stream;
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
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_acceptHeader != null);
		}
	}
}