namespace Linq2Rest.Tests
{
	using System;
	using System.Collections.Specialized;
	using NUnit.Framework;

	[TestFixture]
	public class ModelFilterExtensionsTests
	{
		private FakeItem[] _source;

		[SetUp]
		public void TestSetup()
		{
			_source = new[]
				          {
					          new FakeItem
						          {
							          DoubleValue = 123d
						          }
				          };
		}

		[TestCase("DoubleValue gt blah")]
		[TestCase("DateValue eq 123")]
		[TestCase("DateValue eq datetime'123'")]
		public void WhenFilteringWithInvalidFilterParametersThenThrows(string filter)
		{
			var parameters = new NameValueCollection
				                 {
					                 { "$filter", filter }
				                 };

			Assert.Throws<FormatException>(() => _source.Filter(parameters));
		}

		[TestCase("DoubleValues")]
		[TestCase("Date")]
		[TestCase("'123'")]
		public void WhenFilteringWithInvalidOrderingParametersThenThrows(string sorting)
		{
			var parameters = new NameValueCollection
				                 {
					                 { "$orderby", sorting }
				                 };

			Assert.Throws<FormatException>(() => _source.Filter(parameters));
		}

		[TestCase("DoubleValues")]
		[TestCase("x")]
		[TestCase("'123'")]
		public void WhenFilteringWithInvalidTopParametersThenThrows(string top)
		{
			var parameters = new NameValueCollection
				                 {
					                 { "$top", top }
				                 };

			Assert.Throws<FormatException>(() => _source.Filter(parameters));
		}

		[TestCase("DoubleValues")]
		[TestCase("x")]
		[TestCase("'123'")]
		public void WhenFilteringWithInvalidSkipParametersThenThrows(string top)
		{
			var parameters = new NameValueCollection
				                 {
					                 { "$skip", top }
				                 };

			Assert.Throws<FormatException>(() => _source.Filter(parameters));
		}
	}
}
