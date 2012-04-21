namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class SingleExpressionFactoryTests
	{
		private SingleExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new SingleExpressionFactory();
		}

		[Test]
		public void WhenFilterSingleIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesSingleParameterThenReturnedExpressionContainsSingle()
		{
			var expression = _factory.Convert("1.23");

			Assert.IsAssignableFrom<Single>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesSingleParameterWithTrailingUpperCaseMThenReturnedExpressionContainsSingle()
		{
			var expression = _factory.Convert("1.23F");

			Assert.IsAssignableFrom<Single>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesSingleParameterWithTrailingLowerCaseMThenReturnedExpressionContainsSingle()
		{
			var expression = _factory.Convert("1.23f");

			Assert.IsAssignableFrom<Single>(expression.Value);
		}
	}
}