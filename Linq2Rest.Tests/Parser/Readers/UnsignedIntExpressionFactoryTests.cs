namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedIntExpressionFactoryTests
	{
		private UnsignedIntExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedIntExpressionFactory();
		}

		[Test]
		public void WhenFilterUnsignedIntIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesUnsignedIntParameterThenReturnedExpressionContainsUnsignedInt()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<uint>(expression.Value);
		}
	}
}