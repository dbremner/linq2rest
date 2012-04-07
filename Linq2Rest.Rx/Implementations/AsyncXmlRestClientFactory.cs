namespace Linq2Rest.Reactive.Implementations
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Net;

	/// <summary>
	/// Defines the factory to create a REST client using XML requests.
	/// </summary>
	public class AsyncXmlRestClientFactory : IAsyncRestClientFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncXmlRestClientFactory"/> class.
		/// </summary>
		/// <param name="serviceBase">The base <see cref="Uri"/> for the REST service.</param>
		public AsyncXmlRestClientFactory(Uri serviceBase)
		{
			Contract.Requires<ArgumentNullException>(serviceBase != null);

			ServiceBase = serviceBase;
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
			return new AsyncXmlRestClient(source);
		}

		private class AsyncXmlRestClient : IAsyncRestClient
		{
			private readonly HttpWebRequest _request;

			public AsyncXmlRestClient(Uri uri)
			{
				Contract.Requires(uri != null);

				_request = (HttpWebRequest)WebRequest.Create(uri);
				_request.Accept = "application/Xml";
			}

			public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
			{
				return _request.BeginGetResponse(callback, state);
			}

			public string EndGetResult(IAsyncResult result)
			{
				var response = _request.EndGetResponse(result);
				var reader = new StreamReader(response.GetResponseStream());

				return reader.ReadToEnd();
			}

			[ContractInvariantMethod]
			private void Invariants()
			{
				Contract.Invariant(_request != null);
			}
		}
	}
}