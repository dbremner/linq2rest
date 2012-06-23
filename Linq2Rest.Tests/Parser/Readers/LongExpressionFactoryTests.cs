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
	public class LongExpressionFactoryTests
	{
		private LongExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new LongExpressionFactory();
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenReturnsDefaultValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(default(long), _factory.Convert(Parameter).Value);
		}

		[Test]
		public void WhenFilterIncludesLongParameterThenReturnedExpressionContainsLong()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<long>(expression.Value);
		}
	}
}