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
	public class DateTimeOffsetValueWriterTests
	{
		private DateTimeOffsetValueWriter _writer;

		[SetUp]
		public void Setup()
		{
			_writer = new DateTimeOffsetValueWriter();
		}

		[Test]
		public void WhenWritingDateTimeValueThenWritesString()
		{
			var value = new DateTimeOffset(2012, 5, 6, 16, 11, 00, TimeSpan.FromHours(2));
			var result = _writer.Write(value);

			Assert.AreEqual("datetimeoffset'2012-05-06T16:11:00+02:00'", result);
		}
	}
}