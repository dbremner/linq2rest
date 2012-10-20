// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Globalization;
using System.Threading.Tasks;

namespace Linq2Rest.Reactive.Implementations
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Net;

	/// <summary>
	/// Defines the factory to create a REST client using JSON requests.
	/// </summary>
	public class AsyncJsonRestClientFactory : IAsyncRestClientFactory
	{
		private HttpMethod _method;
		private Stream _input;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncJsonRestClientFactory"/> class.
		/// </summary>
		/// <param name="serviceBase">The base <see cref="Uri"/> for the REST service.</param>
		public AsyncJsonRestClientFactory(Uri serviceBase)
		{
#if !NETFX_CORE
			Contract.Requires<ArgumentNullException>(serviceBase != null);
			Contract.Requires<ArgumentException>(serviceBase.Scheme == Uri.UriSchemeHttp || serviceBase.Scheme == Uri.UriSchemeHttps);
#endif

			ServiceBase = serviceBase;
			_method = HttpMethod.Get;
		}

		/// <summary>
		/// Gets the base service address.
		/// </summary>
		public Uri ServiceBase { get; private set; }

		/// <summary>
		/// Creates an <see cref="IAsyncRestClient"/>.
		/// </summary>
		/// <param name="source">The <see cref="Uri"/> to download from.</param>
		/// <returns>An <see cref="IAsyncRestClient"/> instance.</returns>
		public IAsyncRestClient Create(Uri source)
		{
			return new AsyncJsonRestClient(source, _method, _input);
		}

		public void SetMethod(HttpMethod method)
		{
			_method = method;
		}

		public void SetInput(Stream input)
		{
			_input = input;
		}

		private class AsyncJsonRestClient : IAsyncRestClient
		{
			private readonly Uri _uri;
			private readonly HttpMethod _method;
			private readonly Stream _input;

			public AsyncJsonRestClient(Uri uri, HttpMethod method, Stream input)
			{
#if !NETFX_CORE
				Contract.Requires(uri != null);
				Contract.Requires(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
#endif

				_uri = uri;
				_method = method;
				_input = input;
			}

			public Task<Stream> Download()
			{
				var request = (HttpWebRequest)WebRequest.Create(_uri);
				request.Accept = "application/json";
				request.Method = _method.ToString().ToUpperInvariant();
				if (_method == HttpMethod.Put || _method == HttpMethod.Post)
				{
					return Task<Stream>.Factory
						.FromAsync(
							request.BeginGetRequestStream,
							request.EndGetRequestStream,
							request)
						.ContinueWith(s =>
										  {
											  var buffer = new byte[_input.Length];
											  _input.Read(buffer, 0, buffer.Length);

											  var stream = s.Result;
											  stream.Write(buffer, 0, buffer.Length);
											  return s.AsyncState as HttpWebRequest;
										  })
										  .ContinueWith(
										  r =>
										  {
											  var webRequest = r.Result;
											  return Task<WebResponse>.Factory.FromAsync(
												  webRequest.BeginGetResponse,
												  webRequest.EndGetResponse,
												  null)
												  .ContinueWith(w => w.Result.GetResponseStream())
												  .Result;
										  });
				}

				return Task<WebResponse>.Factory
						.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)
						.ContinueWith(x => x.Result.GetResponseStream());
			}

#if !NETFX_CORE
			[ContractInvariantMethod]
			private void Invariants()
			{
				Contract.Invariant(_uri != null);
			}
#endif
		}
	}
}