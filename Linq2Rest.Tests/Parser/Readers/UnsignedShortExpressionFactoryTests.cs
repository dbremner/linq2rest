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
	public class UnsignedShortExpressionFactoryTests
	{
		private UnsignedShortExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedShortExpressionFactory();
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenReturnsDefaultValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(default(ushort), _factory.Convert(Parameter).Value);
		}

		[Test]
		public void WhenFilterIncludesUnsignedShortParameterThenReturnedExpressionContainsUnsignedShort()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<ushort>(expression.Value);
		}
	}
}