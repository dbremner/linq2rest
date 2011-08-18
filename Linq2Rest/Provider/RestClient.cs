namespace Linq2Rest.Provider
{
	using System;
	using System.Net;

	public class RestClient : IRestClient
	{
		private readonly WebClient _client;

		public RestClient(Uri uri)
		{
			_client = new WebClient();
			_client.Headers["Accept"] = "text/javascript";

			ServiceBase = uri;
		}

		public Uri ServiceBase { get; private set; }

		public string Get(Uri uri)
		{
			return _client.DownloadString(uri);
		}
	}
}