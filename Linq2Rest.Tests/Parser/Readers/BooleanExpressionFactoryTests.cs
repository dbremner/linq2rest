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
	public class BooleanExpressionFactoryTests
	{
		private BooleanExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new BooleanExpressionFactory();
		}

		[Test]
		public void WhenFilterBooleanIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<InvalidOperationException>(() => _factory.Convert(Parameter));
		}

		[Test]
		public void WhenFilterIncludesBooleanParameterAsNumberThenReturnedExpressionContainsBoolean()
		{
			var expression = _factory.Convert("1");

			Assert.IsAssignableFrom<bool>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesBooleanParameterAsWordThenReturnedExpressionContainsBoolean()
		{
			var expression = _factory.Convert("true");

			Assert.IsAssignableFrom<bool>(expression.Value);
		}
	}
}