﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	using Linq2Rest.Parser;

	using NUnit.Framework;

	public class FilterTests
	{
		private FakeItem[] _collection;

		[SetUp]
		public void TestSetup()
		{
			_collection = new[]
				{
					new FakeItem
						{
							ChoiceValue = Choice.This, DateValue = new DateTime(2011, 12, 24), DecimalValue = 2m, DoubleValue = 3d, IntValue = 4, StringValue = "test"
						},
					new FakeItem
						{
							ChoiceValue = Choice.That, DateValue = new DateTime(2011, 01, 24), DecimalValue = 1m, DoubleValue = 2d, IntValue = 3, StringValue = "blah"
						},
					new FakeItem
						{
							ChoiceValue = Choice.Either, DateValue = new DateTime(2012, 01, 01), DecimalValue = 3m, DoubleValue = 4d, IntValue = 5, StringValue = "something"
						}
				};
		}

		[Test]
		public void WhenApplyingSerializedExpressionThenCreatesSameResultAsOriginalExpression()
		{
			Func<FakeItem, bool> original = x => (x.ChoiceValue & Choice.That) == Choice.That && x.IntValue >= 3;

			var factory = new FilterExpressionFactory();
			var deserialized = factory.Create<FakeItem>("ChoiceValue eq That And IntValue ge 3");

			var originalResult = _collection.Where(original).ToArray();
			var deserializedResult = _collection.Where(deserialized.Compile()).ToArray();

			Assert.True(originalResult.SequenceEqual(deserializedResult));
		}
	}
}