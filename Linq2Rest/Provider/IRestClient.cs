// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Provider
{
	using System;
	using System.Net;

	public interface IRestClient
	{
		Uri ServiceBase { get; }

		string Get(Uri uri);
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

		public string Get(Uri uri)
		{
			return _client.DownloadString(uri);
		}
	}
}