namespace Linq2Rest.Tests.Provider.Writers
{
	using System;
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class GuidValueWriterTests
	{
		private GuidValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new GuidValueWriter();
		}

		[Test]
		public void WhenWritingGuidThenEnclosesInSingleQuote()
		{
			var guid = new Guid("e9bc1b54-18fe-4951-a6c6-1de1ef23d6c3");
			var result = _writer.Write(guid);

			Assert.AreEqual("guid'e9bc1b54-18fe-4951-a6c6-1de1ef23d6c3'", result);
		}
	}
}