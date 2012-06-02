//  Copyright © Reimers.dk 2011
//	This source is subject to the Microsoft Public License (Ms-PL).
//	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//	All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
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
	}
}
