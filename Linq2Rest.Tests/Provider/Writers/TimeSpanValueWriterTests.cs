// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanValueWriterTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TimeSpanValueWriterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider.Writers
{
	using System;
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class TimeSpanValueWriterTests
	{
		[SetUp]
		public void Setup()
		{
			_writer = new TimeSpanValueWriter();
		}

		private TimeSpanValueWriter _writer;

		[Test]
		public void WhenWritingShortTimeSpanValueThenWritesString()
		{
			var value = new TimeSpan(2, 15, 0);
			var result = _writer.Write(value);

			Assert.AreEqual("time'PT2H15M'", result);
		}

		[Test]
		public void WhenWritingTimeSpanValueThenWritesString()
		{
			var value = new TimeSpan(2, 2, 15, 0);
			var result = _writer.Write(value);

			Assert.AreEqual("time'P2DT2H15M'", result);
		}
	}
}