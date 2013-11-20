// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedLongValueWriterTests.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the UnsignedLongValueWriterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedLongValueWriterTests
	{
		private UnsignedLongValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new UnsignedLongValueWriter();
		}

		[Test]
		public void WhenWritingUnsignedLongValueThenWritesString()
		{
			var result = _writer.Write((ulong)123);

			Assert.AreEqual("123", result);
		}
	}
}