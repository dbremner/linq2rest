// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Globalization;
	using System.Threading;

	using Linq2Rest.Parser;

	using NUnit.Framework;

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
		[TestCase("IntValue eq 1", "x => (x.IntValue == 1)")]
		[TestCase("IntValue eq (10 mod 2)", "x => (x.IntValue == (10 % 2))")]
		[TestCase("(10 mod 2) eq IntValue", "x => ((10 % 2) == x.IntValue)")]
		[TestCase("IntValue ne 1", "x => (x.IntValue != 1)")]
		[TestCase("IntValue gt 1", "x => (x.IntValue > 1)")]
		[TestCase("IntValue ge 1", "x => (x.IntValue >= 1)")]
		[TestCase("IntValue lt 1", "x => (x.IntValue < 1)")]
		[TestCase("IntValue le 1", "x => (x.IntValue <= 1)")]
		[TestCase("DoubleValue eq 1.2", "x => (x.DoubleValue == 1.2)")]
		[TestCase("DoubleValue eq (10 mod 2)", "x => (x.DoubleValue == (10 % 2))")]
		[TestCase("(10 mod 2) eq DoubleValue", "x => ((10 % 2) == x.DoubleValue)")]
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
		[TestCase("StringValue eq 1", "x => (x.StringValue == \"1\")")]
		[TestCase("StringValue eq '1'", "x => (x.StringValue == \"1\")")]
		[TestCase("StringValue eq 'something'", "x => (x.StringValue == \"something\")")]
		[TestCase("StringValue ne 1", "x => (x.StringValue != \"1\")")]
		[TestCase("StringValue/Length eq 1", "x => (x.StringValue.Length == 1)")]
		[TestCase("StringValue/Length ne 1", "x => (x.StringValue.Length != 1)")]
		[TestCase("substringof('text', StringValue) eq true", "x => ((x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > -1) == True)")]
		[TestCase("substringof('text', StringValue) ne true", "x => ((x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > -1) != True)")]
		[TestCase("substringof('text', StringValue) eq false", "x => ((x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > -1) == False)")]
		[TestCase("substringof('text', StringValue) ne false", "x => ((x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > -1) != False)")]
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
		public void WhenProvidingValidInputThenGetsExpectedExpression(string filter, string expression)
		{
			var result = _factory.Create<FakeItem>(filter);

			Assert.AreEqual(expression, result.ToString(), "Failed for " + filter);
		}
	}
}