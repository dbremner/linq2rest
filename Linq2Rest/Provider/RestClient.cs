// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

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
			
			ServiceBase = uri;
		}

		public Uri ServiceBase { get; private set; }

		public string Get(Uri uri)
		{
			_client.Headers["Accept"] = "application/json";

			return _client.DownloadString(uri);
		}
	}
}