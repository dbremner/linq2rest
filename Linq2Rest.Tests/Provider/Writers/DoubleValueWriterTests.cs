// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class DoubleValueWriterTests
	{
		private DoubleValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new DoubleValueWriter();
		}

		[Test]
		public void WhenWritingDoubleValueThenWritesString()
		{
			var result = _writer.Write(1.23d);

			Assert.AreEqual("1.23", result);
		}
	}
}