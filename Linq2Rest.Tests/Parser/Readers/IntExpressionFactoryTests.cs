namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class IntExpressionFactoryTests
	{
		private IntExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new IntExpressionFactory();
		}

		[Test]
		public void WhenFilterIntIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesIntParameterThenReturnedExpressionContainsInt()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<int>(expression.Value);
		}
	}
}