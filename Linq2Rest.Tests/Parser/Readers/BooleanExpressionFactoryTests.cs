// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser.Readers
{
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
		public void WhenFilterIsIncorrectFormatThenReturnsNullValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(null, _factory.Convert(Parameter).Value);
		}

		[TestCase("1", true)]
		[TestCase("0", false)]
		[TestCase("2", null)]
		public void WhenFilterIncludesBooleanParameterAsNumberThenReturnedExpressionContainsBoolean(string parameter, object value)
		{
			var expression = _factory.Convert(parameter);

			Assert.AreEqual(value, expression.Value);
		}

		[TestCase("true", true)]
		[TestCase("True", true)]
		[TestCase("false", false)]
		[TestCase("False", false)]
		[TestCase("blah", null)]
		public void WhenFilterIncludesBooleanParameterAsWordThenReturnedExpressionContainsBoolean(string parameter, object value)
		{
			var expression = _factory.Convert(parameter);

			Assert.AreEqual(value, expression.Value);
		}
	}
}