namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class StringValueWriterTests
	{
		private StringValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new StringValueWriter();
		}

		[Test]
		public void WhenWritingStringThenEnclosesInSingleQuote()
		{
			var result = _writer.Write("hello world");

			Assert.AreEqual("'hello world'", result);
		}
	}
}
