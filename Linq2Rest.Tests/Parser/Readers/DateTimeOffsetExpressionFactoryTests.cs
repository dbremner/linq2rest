// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using System.Xml;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class DateTimeOffsetExpressionFactoryTests
	{
		private DateTimeOffsetExpressionFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new DateTimeOffsetExpressionFactory();
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenReturnsDefaultValue()
		{
			const string Parameter = "blah";

			Assert.AreEqual(default(DateTimeOffset), _factory.Convert(Parameter).Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeOffsetParameterThenReturnedExpressionContainsDateTimeOffset()
		{
			var dateTimeOffset = new DateTimeOffset(2012, 5, 6, 18, 10, 0, TimeSpan.FromHours(2));
			var parameter = string.Format("datetimeoffset'{0}'", XmlConvert.ToString(dateTimeOffset));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTimeOffset>(expression.Value);
		}

		[Test]
		public void WhenFilterIncludesDateTimeOffsetParameterInDoubleQuotesThenReturnedExpressionContainsDateTimeOffset()
		{
			var dateTimeOffset = new DateTimeOffset(2012, 5, 6, 18, 10, 0, TimeSpan.FromHours(2));
			var parameter = string.Format("datetimeoffset\"{0}\"", XmlConvert.ToString(dateTimeOffset));

			var expression = _factory.Convert(parameter);

			Assert.IsAssignableFrom<DateTimeOffset>(expression.Value);
		}
	}
}