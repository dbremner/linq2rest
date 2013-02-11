// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the FilterExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	using Linq2Rest.Parser;

	using NUnit.Framework;

	[TestFixture]
	public class FilterExpressionFactoryTests
	{
		private FilterExpressionFactory _factory;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			_factory = new FilterExpressionFactory();
		}

		[Test]
		[TestCase("substringof('123456789(ext:1234))', StringValue)", "x => x.StringValue.Contains(\"123456789(ext:1234))\")")]
		[TestCase("StringValue eq 'a \"quote\"'", "x => (x.StringValue == \"a \"quote\"\")")]
		[TestCase("StringValue eq 'a \"quote\" within the text'", "x => (x.StringValue == \"a \"quote\" within the text\")")]
		[TestCase("StringValue eq 'a 'single quote' within the text'", "x => (x.StringValue == \"a 'single quote' within the text\")")]
		[TestCase("true", "x => True")]
		[TestCase("ChoiceValue eq This", "x => ((Convert(x.ChoiceValue) & Convert(This)) == Convert(This))")]
		[TestCase("IntValue eq 1", "x => (x.IntValue == 1)")]
		[TestCase("(IntValue eq 1) and DoubleValue lt 2", "x => ((x.IntValue == 1) AndAlso (x.DoubleValue < 2))")]
		[TestCase("IntValue eq (10 mod 2)", "x => (x.IntValue == (10 % 2))")]
		[TestCase("(10 mod 2) eq IntValue", "x => ((10 % 2) == x.IntValue)")]
		[TestCase("IntValue ne 1", "x => (x.IntValue != 1)")]
		[TestCase("IntValue gt 1", "x => (x.IntValue > 1)")]
		[TestCase("-IntValue lt 1", "x => (-x.IntValue < 1)")]
		[TestCase("IntValue ge 1", "x => (x.IntValue >= 1)")]
		[TestCase("IntValue lt 1", "x => (x.IntValue < 1)")]
		[TestCase("IntValue le 1", "x => (x.IntValue <= 1)")]
		[TestCase("DoubleValue eq 1.2", "x => (x.DoubleValue == 1.2)")]
		[TestCase("DoubleValue eq (10 mod 2)", "x => (x.DoubleValue == (10 % 2))")]
		[TestCase("(10 mod 2) eq DoubleValue", "x => ((10 % 2) == x.DoubleValue)")]
		[TestCase("(DoubleValue mod 2) eq 10", "x => ((x.DoubleValue % 2) == 10)")]
		[TestCase("DoubleValue mod 2 eq 10", "x => ((x.DoubleValue % 2) == 10)")]
		[TestCase("DoubleValue ne 1.2", "x => (x.DoubleValue != 1.2)")]
		[TestCase("DoubleValue gt 1.2", "x => (x.DoubleValue > 1.2)")]
		[TestCase("DoubleValue ge 1.2", "x => (x.DoubleValue >= 1.2)")]
		[TestCase("DoubleValue lt 1.2", "x => (x.DoubleValue < 1.2)")]
		[TestCase("DoubleValue le 1.2", "x => (x.DoubleValue <= 1.2)")]
		[TestCase("DoubleValue eq 1", "x => (x.DoubleValue == 1)")]
		[TestCase("DoubleValue ne 1", "x => (x.DoubleValue != 1)")]
		[TestCase("DoubleValue gt 1", "x => (x.DoubleValue > 1)")]
		[TestCase("DoubleValue ge 1", "x => (x.DoubleValue >= 1)")]
		[TestCase("DoubleValue lt 1", "x => (x.DoubleValue < 1)")]
		[TestCase("DoubleValue le 1", "x => (x.DoubleValue <= 1)")]
		[TestCase("(DoubleValue add 2) eq 3", "x => ((x.DoubleValue + 2) == 3)")]
		[TestCase("(DoubleValue sub 2) eq 3", "x => ((x.DoubleValue - 2) == 3)")]
		[TestCase("(DoubleValue mul 2) eq 3", "x => ((x.DoubleValue * 2) == 3)")]
		[TestCase("(DoubleValue div 2) eq 3", "x => ((x.DoubleValue / 2) == 3)")]
		[TestCase("DoubleValue add 2 eq 3", "x => ((x.DoubleValue + 2) == 3)")]
		[TestCase("DoubleValue sub 2 eq 3", "x => ((x.DoubleValue - 2) == 3)")]
		[TestCase("DoubleValue mul 2 eq 3", "x => ((x.DoubleValue * 2) == 3)")]
		[TestCase("DoubleValue div 2 eq 3", "x => ((x.DoubleValue / 2) == 3)")]
		[TestCase("(DoubleValue div 2) mod 2 eq 3", "x => (((x.DoubleValue / 2) % 2) == 3)")]
		[TestCase("StringValue eq 1", "x => (x.StringValue == \"1\")")]
		[TestCase("StringValue eq '1'", "x => (x.StringValue == \"1\")")]
		[TestCase("StringValue eq 'something'", "x => (x.StringValue == \"something\")")]
		[TestCase("StringValue eq 'this and that'", "x => (x.StringValue == \"this and that\")")]
		[TestCase("StringValue eq 'Group1 foo Group2'", "x => (x.StringValue == \"Group1 foo Group2\")")]
		[TestCase("StringValue eq 'Group1 and Group2'", "x => (x.StringValue == \"Group1 and Group2\")")]
		[TestCase("StringValue eq 'Group1 or Group2'", "x => (x.StringValue == \"Group1 or Group2\")")]
		[TestCase("StringValue eq 'Group1 not Group2'", "x => (x.StringValue == \"Group1 not Group2\")")]
		[TestCase("StringValue ne 1", "x => (x.StringValue != \"1\")")]
		[TestCase("StringValue/Length eq 1", "x => (x.StringValue.Length == 1)")]
		[TestCase("StringValue/Length ne 1", "x => (x.StringValue.Length != 1)")]
		[TestCase("substringof('text', StringValue) eq true", "x => (x.StringValue.Contains(\"text\") == True)")]
		[TestCase("substringof('text', StringValue) ne true", "x => (x.StringValue.Contains(\"text\") != True)")]
		[TestCase("substringof('text', StringValue) eq false", "x => (x.StringValue.Contains(\"text\") == False)")]
		[TestCase("substringof('text', StringValue) ne false", "x => (x.StringValue.Contains(\"text\") != False)")]
		[TestCase("endswith(StringValue, 'text') eq true", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) == True)")]
		[TestCase("endswith(StringValue, 'text') ne true", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) != True)")]
		[TestCase("endswith(StringValue, 'text') eq false", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) == False)")]
		[TestCase("endswith(StringValue, 'text') ne false", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) != False)")]
		[TestCase("startswith(StringValue, 'text') eq true", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) == True)")]
		[TestCase("startswith(StringValue, 'text') ne true", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) != True)")]
		[TestCase("startswith(StringValue, 'text') eq false", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) == False)")]
		[TestCase("startswith(StringValue, 'text') ne false", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) != False)")]
		[TestCase("not length(StringValue) eq 1", "x => Not((x.StringValue.Length == 1))")]
		[TestCase("length(StringValue) eq 1", "x => (x.StringValue.Length == 1)")]
		[TestCase("length(StringValue) ne 1", "x => (x.StringValue.Length != 1)")]
		[TestCase("length(StringValue) gt 1", "x => (x.StringValue.Length > 1)")]
		[TestCase("length(StringValue) ge 1", "x => (x.StringValue.Length >= 1)")]
		[TestCase("length(StringValue) lt 1", "x => (x.StringValue.Length < 1)")]
		[TestCase("length(StringValue) le 1", "x => (x.StringValue.Length <= 1)")]
		[TestCase("indexof(StringValue, 'text') eq 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) == 1)")]
		[TestCase("indexof(StringValue, 'text') ne 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) != 1)")]
		[TestCase("indexof(StringValue, 'text') gt 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > 1)")]
		[TestCase("indexof(StringValue, 'text') ge 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) >= 1)")]
		[TestCase("indexof(StringValue, 'text') lt 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) < 1)")]
		[TestCase("indexof(StringValue, 'text') le 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) <= 1)")]
		[TestCase("indexof('text', StringValue) eq 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) == 1)")]
		[TestCase("indexof('text', StringValue) ne 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) != 1)")]
		[TestCase("indexof('text', StringValue) gt 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) > 1)")]
		[TestCase("indexof('text', StringValue) ge 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) >= 1)")]
		[TestCase("indexof('text', StringValue) lt 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) < 1)")]
		[TestCase("indexof('text', StringValue) le 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) <= 1)")]
		[TestCase("substring(StringValue, 1) eq 'text'", "x => (x.StringValue.Substring(1) == \"text\")")]
		[TestCase("substring(StringValue, 1) ne 'text'", "x => (x.StringValue.Substring(1) != \"text\")")]
		[TestCase("substring(StringValue, 1) ne 'text' and IntValue eq 25", "x => ((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25))")]
		[TestCase("substring(StringValue, 1) ne 'text' and IntValue eq 25 and DoubleValue le 10", "x => (((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25)) AndAlso (x.DoubleValue <= 10))")]
		[TestCase("tolower(StringValue) ne 'text'", "x => (x.StringValue.ToLowerInvariant() != \"text\")")]
		[TestCase("tolower(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.ToLowerInvariant() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
		[TestCase("toupper(StringValue) ne 'text'", "x => (x.StringValue.ToUpperInvariant() != \"text\")")]
		[TestCase("toupper(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.ToUpperInvariant() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
		[TestCase("trim(StringValue) ne 'text'", "x => (x.StringValue.Trim() != \"text\")")]
		[TestCase("trim(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.Trim() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
		[TestCase("hour(DateValue) eq 2", "x => (x.DateValue.Hour == 2)")]
		[TestCase("minute(DateValue) eq 2", "x => (x.DateValue.Minute == 2)")]
		[TestCase("second(DateValue) eq 2", "x => (x.DateValue.Second == 2)")]
		[TestCase("day(DateValue) eq 2", "x => (x.DateValue.Day == 2)")]
		[TestCase("month(DateValue) eq 2", "x => (x.DateValue.Month == 2)")]
		[TestCase("year(DateValue) eq 2011", "x => (x.DateValue.Year == 2011)")]
		[TestCase("round(DoubleValue) gt 1", "x => (Round(x.DoubleValue) > 1)")]
		[TestCase("floor(DoubleValue) gt 1", "x => (Floor(x.DoubleValue) > 1)")]
		[TestCase("ceiling(DoubleValue) gt 1", "x => (Ceiling(x.DoubleValue) > 1)")]
		[TestCase("round(DecimalValue) gt 1", "x => (Round(x.DecimalValue) > 1)")]
		[TestCase("floor(DecimalValue) gt 1", "x => (Floor(x.DecimalValue) > 1)")]
		[TestCase("ceiling(DecimalValue) gt 1", "x => (Ceiling(x.DecimalValue) > 1)")]
		[TestCase("(StringValue ne 'text') or IntValue gt 2", "x => ((x.StringValue != \"text\") OrElse (x.IntValue > 2))")]
		[TestCase("(startswith(tolower(StringValue),'foo') eq true and endswith(tolower(StringValue),'1') eq true) and (tolower(StringValue) eq 'bar03')", "x => (((x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) == True) AndAlso (x.StringValue.ToLowerInvariant().EndsWith(\"1\", OrdinalIgnoreCase) == True)) AndAlso (x.StringValue.ToLowerInvariant() == \"bar03\"))")]
		[TestCase("(startswith(tolower(StringValue),'foo') and endswith(tolower(StringValue),'1')) and (tolower(StringValue) eq 'bar03')", "x => ((x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) AndAlso x.StringValue.ToLowerInvariant().EndsWith(\"1\", OrdinalIgnoreCase)) AndAlso (x.StringValue.ToLowerInvariant() == \"bar03\"))")]
		[TestCase("startswith(tolower(StringValue),'foo')", "x => x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase)")]
		[TestCase("StringValue/Length eq 5 and Children/any(a: a/ChildStringValue eq 'foo')", "x => ((x.StringValue.Length == 5) AndAlso x.Children.Any(a => (a.ChildStringValue == \"foo\")))")]
		[TestCase("Children/any(a: a/ChildStringValue eq 'foo')", "x => x.Children.Any(a => (a.ChildStringValue == \"foo\"))")]
		[TestCase("Children/all(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.All(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))")]
		[TestCase("Children/all(y: y/Children/any(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.All(y => y.Children.Any(z => (z.GrandChildStringValue == \"foo\")))")]
		[TestCase("Children/any(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.Any(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))")]
		[TestCase("Children/any(a: startswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.Any(a => a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase))")]
		[TestCase("Children/all(a: startswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.All(a => a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase))")]
		[TestCase("Children/all(a: startswith(tolower(a/ChildStringValue), 'foo') and endswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.All(a => (a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) AndAlso a.ChildStringValue.ToLowerInvariant().EndsWith(\"foo\", OrdinalIgnoreCase)))")]
		[TestCase("Children/any(a: a/Children/any(b: startswith(tolower(b/GrandChildStringValue), 'foo')))", "x => x.Children.Any(a => a.Children.Any(b => b.GrandChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase)))")]
		[TestCase("Children/any(a: startswith(tolower(a/ChildStringValue), StringValue))", "x => x.Children.Any(a => a.ChildStringValue.ToLowerInvariant().StartsWith(x.StringValue, OrdinalIgnoreCase))")]
		[TestCase("Children/all(y: y/ID eq 2 add ID)", "x => x.Children.All(y => (y.ID == (2 + x.ID)))")]
		[TestCase("Child/Attributes/all(y: y eq 'blah')", "x => x.Child.Attributes.All(y => (y == \"blah\"))")]
		[TestCase("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)")]
		[TestCase("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)")]
		[TestCase("Duration eq time'PT2H15M'", "x => (x.Duration == 02:15:00)")]
		[TestCase("PointInTime eq datetimeoffset'2012-05-06T18:10:00+02:00'", "x => (x.PointInTime == 5/6/2012 6:10:00 PM +02:00)")]
		public void WhenProvidingValidInputThenGetsExpectedExpression(string filter, string expression)
		{
			var result = _factory.Create<FakeItem>(filter);

			Assert.AreEqual(expression, result.ToString(), "Failed for " + filter);
		}

		[Test]
		public void CanHandleParsedValues()
		{
			var result = _factory.Create<ParseParent>("Item eq 1 and Number le 2");

			Assert.AreEqual("x => ((x.Item == Parse(\"1\")) AndAlso (x.Number <= 2))", result.ToString());
		}

		[Test]
		public void FilteringIsCaseSensitive()
		{
			var items = new[] { new FakeItem { StringValue = "blah blah" } };
			var filter = _factory.Create<FakeItem>("substringof('Blah', StringValue)");

			var result = items.Where(filter.Compile());

			Assert.IsEmpty(result);
		}

		[Test]
		public void CanHandleEqualComparisonWithEmptyGuid()
		{
			var result = _factory.Create<FakeItem>("GlobalID eq guid'00000000-0000-0000-0000-000000000000'");

			Assert.AreEqual("x => (x.GlobalID == 00000000-0000-0000-0000-000000000000)", result.ToString());
		}

		[Test]
		public void CanHandleNotEqualComparisonWithEmptyGuid()
		{
			var result = _factory.Create<FakeItem>("GlobalID ne guid'00000000-0000-0000-0000-000000000000'");

			Assert.AreEqual("x => (x.GlobalID != 00000000-0000-0000-0000-000000000000)", result.ToString());
		}

		[TestCase("DateValue eq 123", typeof(FormatException))]
		[TestCase("DateValue eq datetime'123'", typeof(FormatException))]
		[TestCase("PointInTime eq 'nothing'", typeof(InvalidOperationException))]
		[TestCase("DoubleValue ge 'nothing'", typeof(InvalidOperationException))]
		[TestCase("IntValue lt 'nothing'", typeof(InvalidOperationException))]
		[TestCase("IntValue lt ('nothing' add 1)", typeof(InvalidOperationException))]
		[TestCase("blah", typeof(InvalidOperationException))]
		[TestCase("StringValue not foo", typeof(InvalidOperationException))]
		[TestCase("'StringValue' not foo", typeof(InvalidOperationException))]
		[TestCase("StringValue gt foo", typeof(InvalidOperationException))]
		[TestCase("-StringValue eq 'blah'", typeof(InvalidOperationException))]
		[TestCase("not DateValue", typeof(InvalidOperationException))]
		[TestCase("not DoubleValue", typeof(InvalidOperationException))]
		[TestCase("Not DoubleValue", typeof(InvalidOperationException))]
		[TestCase("Duration eq time'PT2H15M' and Not DoubleValue", typeof(InvalidOperationException))]
		[TestCase("\0\0", typeof(InvalidOperationException))]
		public void WhenParsingInvalidExpressionThenThrows(string filter, Type exceptionType)
		{
			Assert.Throws(exceptionType, () => _factory.Create<FakeItem>(filter));
		}
	}
}