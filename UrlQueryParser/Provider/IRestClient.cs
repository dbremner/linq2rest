namespace UrlQueryParser.Provider
{
	using System;
	using System.Net;

	public interface IRestClient
	{
		Uri ServiceBase { get; }

		string GetResponse(Uri uri);
	}

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

		public string GetResponse(Uri uri)
		{
			return _client.DownloadString(uri);
		}
	}
}