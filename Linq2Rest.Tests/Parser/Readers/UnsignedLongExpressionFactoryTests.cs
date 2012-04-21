namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedLongExpressionFactoryTests
	{
		private UnsignedLongExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedLongExpressionFactory();
		}

		[Test]
		public void WhenFilterUnsignedLongIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesUnsignedLongParameterThenReturnedExpressionContainsUnsignedLong()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<ulong>(expression.Value);
		}
	}
}