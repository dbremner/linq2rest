// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedLongExpressionFactoryTests
	{
		private UnsignedLongExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedLongExpressionFactory();
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}

		[Test]
		public void WhenFilterIncludesUnsignedLongParameterThenReturnedExpressionContainsUnsignedLong()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<ulong>(expression.Value);
		}
	}
}