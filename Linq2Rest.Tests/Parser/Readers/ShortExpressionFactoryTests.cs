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
	public class ShortExpressionFactoryTests
	{
		private ShortExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new ShortExpressionFactory();
		}

		[Test]
		public void WhenFilterShortIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesShortParameterThenReturnedExpressionContainsShort()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<short>(expression.Value);
		}
	}
}