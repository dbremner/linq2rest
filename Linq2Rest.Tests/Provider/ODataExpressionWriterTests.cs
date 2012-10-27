//  Copyright © Reimers.dk 2012
//	This source is subject to the Microsoft Public License (Ms-PL).
//	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//	All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using NUnit.Framework;

	[TestFixture]
	public class ODataExpressionWriterTests
	{
		[Test]
		public void ConvertsExpressionToString()
		{
			var converter = new ODataExpressionConverter();
			Expression<Func<ChildDto, bool>> expression = x => x.Name == "blah";

			var serialized = converter.Convert(expression);

			Assert.AreEqual("Name eq 'blah'", serialized);
		}

		[Test]
		public void ConvertsExpressionToString2()
		{
			var converter = new ODataExpressionConverter();
			Expression<Func<ChildDto, bool>> expression = x => x.Name.Length + (1 + 1) == 7;

			var serialized = converter.Convert(expression);

			Assert.AreEqual("length(Name) add 2 eq 7", serialized);
		}

		[Test]
		public void CanSerializeEmptyGuid()
		{
			var converter = new ODataExpressionConverter();
			Expression<Func<ChildDto, bool>> expression = x => x.GlobalID != Guid.Empty;

			var serialized = converter.Convert(expression);

			Assert.AreEqual("GlobalID ne guid'00000000-0000-0000-0000-000000000000'", serialized);
		}

		[Test]
		public void CanFilterOnSubCollection()
		{
			var converter = new ODataExpressionConverter();
			Expression<Func<FakeItem, bool>> expression = x => x.Child.Attributes.Any(y => y == "blah");

			var serialized = converter.Convert(expression);

			Assert.AreEqual("Child/Attributes/any(y: y eq 'blah')", serialized);
		}
	}
}
