namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class DoubleExpressionFactoryTests
	{
		private DoubleExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new DoubleExpressionFactory();
		}

		[Test]
		public void WhenFilterDoubleIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesDoubleParameterThenReturnedExpressionContainsDouble()
		{
			var expression = _factory.Convert("1.23");

			Assert.IsAssignableFrom<double>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDoubleParameterWithTrailingUpperCaseMThenReturnedExpressionContainsDouble()
		{
			var expression = _factory.Convert("1.23D");

			Assert.IsAssignableFrom<double>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDoubleParameterWithTrailingLowerCaseMThenReturnedExpressionContainsDouble()
		{
			var expression = _factory.Convert("1.23d");

			Assert.IsAssignableFrom<double>(expression.Value);
		}
	}
}