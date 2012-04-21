// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class ByteValueWriterTests
	{
		private ByteValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new ByteValueWriter();
		}

		[Test]
		public void WhenWritingByteValueThenWritesString()
		{
			const byte Value = 255;
			var result = _writer.Write(Value);

			Assert.AreEqual("FF", result);
		}
	}
}