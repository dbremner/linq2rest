// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedShortValueWriterTests
	{
		private UnsignedShortValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new UnsignedShortValueWriter();
		}

		[Test]
		public void WhenWritingUnsignedShortValueThenWritesString()
		{
			var result = _writer.Write((ushort)123);

			Assert.AreEqual("123", result);
		}
	}
}