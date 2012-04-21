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
	public class DateTimeExpressionFactoryTests
	{
		private DateTimeExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new DateTimeExpressionFactory();
		}

		[Test]
		public void WhenFilterDateTimeIsIncorrectFormatThenThrows()
		{
			const string Parameter = "datetime'blah'";

			Assert.Throws<InvalidOperationException>(() => _factory.Convert(Parameter));
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterThenReturnedExpressionContainsDateTime()
		{
			var dateTime = DateTime.UtcNow;
			var parameter = string.Format("datetime'{0}'", dateTime.ToString("yyyy-MM-ddThh:mm:ssZ"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTime>(((ConstantExpression)expression).Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterInDoubleQuotesThenReturnedExpressionContainsDateTime()
		{
			var dateTime = DateTime.Now;
			var parameter = string.Format("datetime\"{0}\"", dateTime.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ"));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTime>(((ConstantExpression)expression).Value);
		}
	}
}