namespace UrlQueryParser.Tests.Provider
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	using NUnit.Framework;

	using UrlQueryParser.Provider;

	public class RestQueryProviderTests
	{
		private readonly RestContext<string> _provider;

		public RestQueryProviderTests() { _provider = new RestContext<string>(); }

		[Test]
		public void CanProcessExpression()
		{
			var result = _provider.Query.OfType<string>().Where(x => x.Length <= 3).Skip(1).Take(1).Count();
		}
	}
}
