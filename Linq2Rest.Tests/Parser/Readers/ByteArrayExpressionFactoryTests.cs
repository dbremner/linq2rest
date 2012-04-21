namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class ByteArrayExpressionFactoryTests
	{
		private ByteArrayExpressionFactory _factory;
		private const string Base64 = "TWFuIGlzIG/pc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";

		[SetUp]
		public void Setup()
		{
			_factory = new ByteArrayExpressionFactory();
		}

		[Test]
		public void WhenFilterArrayIsIncorrectFormatThenThrows()
		{
			Assert.Throws<InvalidOperationException>(() => _factory.Convert("blah"));
		}

		[Test]
		public void WhenFilterIncludesBinaryParameterWithPrefixBinaryThenReturnedExpressionContainsByteArray()
		{
			var expression = _factory.Convert(string.Format("binary'{0}'", Base64));

			Assert.IsAssignableFrom<byte[]>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesBinaryParameterWithPrefixXThenReturnedExpressionContainsByteArray()
		{
			var expression = _factory.Convert(string.Format("X'{0}'", Base64));

			Assert.IsAssignableFrom<byte[]>(expression.Value);
		}
	}
}