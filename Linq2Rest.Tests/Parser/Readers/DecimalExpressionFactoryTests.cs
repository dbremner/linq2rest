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
	public class DecimalExpressionFactoryTests
	{
		private DecimalExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new DecimalExpressionFactory();
		}

		[Test]
		public void WhenFilterDecimalIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
		
		[Test]
		public void WhenFilterIncludesDecimalParameterThenReturnedExpressionContainsDecimal()
		{
			var expression = _factory.Convert("1.23");

			Assert.IsAssignableFrom<decimal>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDecimalParameterWithTrailingUpperCaseMThenReturnedExpressionContainsDecimal()
		{
			var expression = _factory.Convert("1.23M");

			Assert.IsAssignableFrom<decimal>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDecimalParameterWithTrailingLowerCaseMThenReturnedExpressionContainsDecimal()
		{
			var expression = _factory.Convert("1.23m");

			Assert.IsAssignableFrom<decimal>(expression.Value);
		}
	}
}