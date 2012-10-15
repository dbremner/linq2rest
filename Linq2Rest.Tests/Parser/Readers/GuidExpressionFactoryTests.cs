// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using System.Linq.Expressions;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class GuidExpressionFactoryTests
	{
		private GuidExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new GuidExpressionFactory();
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}

		[Test]
		public void WhenFilterIncludesGuidParameterThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid'{0}'", guid);

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterWithNoDashesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid'{0}'", guid.ToString("N"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterInDoubleQuotesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid\"{0}\"", guid);

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesGuidParameterWithNoDashesInDoubleQuotesThenReturnedExpressionContainsGuid()
		{
			var guid = Guid.NewGuid();
			var parameter = string.Format("guid\"{0}\"", guid.ToString("N"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<Guid>(expression.Value);
		}
	}
}
