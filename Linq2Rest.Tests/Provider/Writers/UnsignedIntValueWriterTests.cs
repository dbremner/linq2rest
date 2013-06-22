// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedIntValueWriterTests.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the UnsignedIntValueWriterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider.Writers
{
	using Linq2Rest.Provider.Writers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedIntValueWriterTests
	{
		[SetUp]
		public void Setup()
		{
			_writer = new UnsignedIntValueWriter();
		}

		private UnsignedIntValueWriter _writer;

		[Test]
		public void WhenWritingUnsignedIntValueThenWritesString()
		{
			var result = _writer.Write((uint)123);

			Assert.AreEqual("123", result);
		}
	}
}