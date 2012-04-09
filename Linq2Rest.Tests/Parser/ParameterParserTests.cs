﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Collections.Specialized;
	using System.Linq;
	using Linq2Rest.Parser;
	using NUnit.Framework;

	[TestFixture]
	public class ParameterParserTests
	{
		private ParameterParser<FakeItem> _parser;

		private FakeItem[] _items;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var nameResolver = new MemberNameResolver();
			_parser = new ParameterParser<FakeItem>(
				new FilterExpressionFactory(),
				new SortExpressionFactory(),
				new SelectExpressionFactory<FakeItem>(nameResolver, new RuntimeTypeProvider(nameResolver)));

			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 2 }, 
					new FakeItem { IntValue = 1, DoubleValue = 1 }, 
					new FakeItem { IntValue = 3, DoubleValue = 3 }
				};
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsNoSystemParametersThenReturnedModelFilterWithoutFiltering(bool useModelFilter)
		{
			var collection = new NameValueCollection();
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(3, filteredItems.Count());
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterAndSortThenReturnedModelFilterFilteringAndSortedByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue ge 1" }, { "$orderby", "IntValue desc" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(3, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(1, filteredItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterSortSkipAndTopThenReturnedModelFilterFilteringFindsItem(bool useModelFilter)
		{
			var collection = new NameValueCollection
			                 	{
			                 		{ "$filter", "IntValue ge 1" },
			                 		{ "$skip", "1" },
			                 		{ "$top", "1" },
			                 		{ "$orderby", "IntValue desc" }
			                 	};
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, filteredItems.Length);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue eq 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsFilterParameterAndApplyingAsExtensionMethodThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "IntValue eq 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsMathFunctionFilterParameterThenReturnedModelFilterFilteringByValue(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$filter", "floor(DoubleValue) gt 1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(2, filteredItems.Count());
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsSkipParameterThenReturnedModelFilterSkippingItems(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$skip", "2" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void WhenRequestContainsTopParameterThenReturnedModelFilterWithTopItems(bool useModelFilter)
		{
			var collection = new NameValueCollection { { "$top", "1" } };
			var filteredItems = GetFilteredItems(useModelFilter, collection);

			Assert.AreEqual(1, filteredItems.Count());
		}

		private object[] GetFilteredItems(bool useModelFilter, NameValueCollection collection)
		{
			var filteredItems = useModelFilter
									? GetModelFilter(collection).Filter(_items)
									: _items.Filter(collection);
			return filteredItems.ToArray();
		}

		private IModelFilter<FakeItem> GetModelFilter(NameValueCollection parameters)
		{
			var filter = _parser.Parse(parameters);
			return filter;
		}
	}
}
