namespace UrlQueryParser.Tests.Provider
{
	using System;
	using System.Linq;

	using NUnit.Framework;

	using UrlQueryParser.Provider;

	public class RestQueryProviderTests
	{
		private readonly RestContext<string> _provider;

		public RestQueryProviderTests() { _provider = new RestContext<string>(new Uri("http://ws.reimers.dk")); }

		[Test]
		public void CanProcessExpression()
		{
			var result = _provider.Query.Where(x => x.Length <= 3).Skip(1).Take(1).Count();
		}
	}
}
