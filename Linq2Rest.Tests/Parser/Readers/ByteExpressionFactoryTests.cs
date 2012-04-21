namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class ByteExpressionFactoryTests
	{
		private ByteExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new ByteExpressionFactory();
		}

		[Test]
		public void WhenFilterByteIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesByteParameterThenReturnedExpressionContainsByte()
		{
			var expression = _factory.Convert("12");

			Assert.IsAssignableFrom<Byte>(expression.Value);
		}
		
		[Test]
		public void WhenFilterIncludesByteParameterInHexFormatThenReturnedExpressionContainsByte()
		{
			var expression = _factory.Convert("f2");

			Assert.IsAssignableFrom<Byte>(expression.Value);
		}
	}
}