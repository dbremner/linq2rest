// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LongValueWriterTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the LongValueWriterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class LongValueWriterTests
	{
		[SetUp]
		public void Setup()
		{
			_writer = new LongValueWriter();
		}

		private LongValueWriter _writer;

		[Test]
		public void WhenWritingLongValueThenWritesString()
		{
			var result = _writer.Write(123L);

			Assert.AreEqual("123", result);
		}
	}
}