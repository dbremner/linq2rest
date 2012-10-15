// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider.Writers
{
	using System;
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class TimeSpanValueWriterTests
	{
		private TimeSpanValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new TimeSpanValueWriter();
		}

		[Test]
		public void WhenWritingTimeSpanValueThenWritesString()
		{
			var value = new TimeSpan(2, 2, 15, 0);
			var result = _writer.Write(value);

			Assert.AreEqual("time'P2DT2H15M'", result);
		}

		[Test]
		public void WhenWritingShortTimeSpanValueThenWritesString()
		{
			var value = new TimeSpan(2, 15, 0);
			var result = _writer.Write(value);

			Assert.AreEqual("time'PT2H15M'", result);
		}
	}
}