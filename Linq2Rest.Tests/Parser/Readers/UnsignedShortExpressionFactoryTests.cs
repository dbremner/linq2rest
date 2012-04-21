namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedShortExpressionFactoryTests
	{
		private UnsignedShortExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedShortExpressionFactory();
		}

		[Test]
		public void WhenFilterUnsignedShortIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesUnsignedShortParameterThenReturnedExpressionContainsUnsignedShort()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<ushort>(expression.Value);
		}
	}
}