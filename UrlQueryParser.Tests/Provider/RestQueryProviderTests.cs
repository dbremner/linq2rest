namespace UrlQueryParser.Tests.Provider
{
	using System;
	using System.Dynamic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.Script.Serialization;
	using NUnit.Framework;
	using UrlQueryParser.Provider;

	public class RestQueryProviderTests
	{
		private readonly RestContext<SimpleDto> _provider;

		public RestQueryProviderTests()
		{
			var serializer = new JavaScriptSerializer();

			_provider = new RestContext<SimpleDto>(
				new RestClient(new Uri("http://localhost:33198/Simple/")), serializer);
		}

		[Test]
		public void CanProcessExpression()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Count(x => x.ID != 0);

			Assert.Equals(1, result);
			//Assert.False(result.Any(x => x.Value == 0));
		}

		private class SelectionObject : DynamicObject{}
	}

	public class SimpleDto
	{
		public int ID { get; set; }

		public string Content { get; set; }

		public double Value { get; set; }
	}
}
