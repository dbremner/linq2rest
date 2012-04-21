namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedIntValueWriterTests
	{
		private UnsignedIntValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new UnsignedIntValueWriter();
		}

		[Test]
		public void WhenWritingUnsignedIntValueThenWritesString()
		{
			var result = _writer.Write(123);

			Assert.AreEqual("123", result);
		}
	}
}