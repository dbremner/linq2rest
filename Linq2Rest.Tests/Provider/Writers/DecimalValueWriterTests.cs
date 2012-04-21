namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class DecimalValueWriterTests
	{
		private DecimalValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new DecimalValueWriter();
		}

		[Test]
		public void WhenWritingDecimalValueThenWritesString()
		{
			var result = _writer.Write(1.23m);

			Assert.AreEqual("1.23", result);
		}
	}
}
