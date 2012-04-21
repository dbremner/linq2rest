namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class BooleanValueWriterTests
	{
		private BooleanValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new BooleanValueWriter();
		}

		[Test]
		public void WhenWritingBooleanThenEnclosesInSingleQuote()
		{
			var result = _writer.Write(true);

			Assert.AreEqual("true", result);
		}
	}
}