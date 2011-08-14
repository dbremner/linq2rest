namespace UrlQueryParser.Tests.Provider
{
	using System;

	using NUnit.Framework;

	using UrlQueryParser.Provider;

	public class RestClientTests
	{
		[Test]
		public void WhenCreatingRestClientThenSetsServiceBase()
		{
			var uri = new Uri("http://localhost");

			var client = new RestClient(uri);

			Assert.AreEqual(uri, client.ServiceBase);
		}
	}
}
