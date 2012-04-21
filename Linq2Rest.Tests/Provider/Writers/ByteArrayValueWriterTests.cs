namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class ByteArrayValueWriterTests
	{
		private ByteArrayValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new ByteArrayValueWriter();
		}

		[Test]
		public void WhenWritingByteArrayThenEnclosesInSingleQuote()
		{
			var byteArray = new byte[] { 1, 2, 3, 4 };
			var result = _writer.Write(byteArray);

			Assert.AreEqual("X'AQIDBA=='", result);
		}
	}
}