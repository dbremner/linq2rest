namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class LongValueWriterTests
	{
		private LongValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new LongValueWriter();
		}

		[Test]
		public void WhenWritingLongValueThenWritesString()
		{
			var result = _writer.Write(123L);

			Assert.AreEqual("123", result);
		}
	}
}