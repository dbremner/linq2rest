namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class LongExpressionFactoryTests
	{
		private LongExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new LongExpressionFactory();
		}

		[Test]
		public void WhenFilterLongIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesLongParameterThenReturnedExpressionContainsLong()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<long>(expression.Value);
		}
	}
}