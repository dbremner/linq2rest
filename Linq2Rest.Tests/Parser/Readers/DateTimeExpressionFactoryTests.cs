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
		private DateTime _dateTime;

		[SetUp]
		public void Setup()
		{
			_factory = new DateTimeExpressionFactory();
			_dateTime = new DateTime(2012, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenReturnsDefaultValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(default(DateTime), _factory.Convert(Parameter).Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterWithZuluThenReturnedExpressionContainsUtcDateTime()
		{
			var utcTime = _dateTime.ToUniversalTime();
			var parameter = string.Format("datetime'{0}'", utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(utcTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterWithZuluInDoubleQuotesThenReturnedExpressionContainsUtcDateTime()
		{
			var utcTime = _dateTime.ToUniversalTime();
			var parameter = string.Format("datetime\"{0}\"", utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

			var expression = _factory.Convert(parameter);
			
			Assert.AreEqual(utcTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterThenReturnedExpressionContainsDateTime()
		{
			var parameter = string.Format("datetime'{0}'", _dateTime.ToString("yyyy-MM-ddThh:mm:ss"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(_dateTime, expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeParameterInDoubleQuotesThenReturnedExpressionContainsDateTime()
		{
			var parameter = string.Format("datetime\"{0}\"", _dateTime.ToString("yyyy-MM-ddThh:mm:ss"));

			var expression = _factory.Convert(parameter);

			Assert.AreEqual(_dateTime, expression.Value);
		}
	}
}