namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class SingleValueWriterTests
	{
		private SingleValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new SingleValueWriter();
		}

		[Test]
		public void WhenWritingSingleValueThenWritesString()
		{
			var result = _writer.Write(1.23f);

			Assert.AreEqual("1.23", result);
		}
	}
}