namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedLongValueWriterTests
	{
		private UnsignedLongValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new UnsignedLongValueWriter();
		}

		[Test]
		public void WhenWritingUnsignedLongValueThenWritesString()
		{
			var result = _writer.Write(123L);

			Assert.AreEqual("123", result);
		}
	}
}