namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class IntValueWriterTests
	{
		private IntValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new IntValueWriter();
		}

		[Test]
		public void WhenWritingIntValueThenWritesString()
		{
			var result = _writer.Write(123);

			Assert.AreEqual("123", result);
		}
	}
}