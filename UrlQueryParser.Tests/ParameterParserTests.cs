namespace UrlQueryParser.Tests
{
	using System.Collections.Specialized;
	using System.Linq;

	using NUnit.Framework;

	using UrlQueryParser.Mvc;
	using UrlQueryParser.Parser;

	public class ParameterParserTests
	{
		private ParameterParser _parser;

		private FakeItem[] _items;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			_parser = new ParameterParser(new FilterExpressionFactory(), new SortExpressionFactory());
			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 2 }, 
					new FakeItem { IntValue = 1, DoubleValue = 1 }, 
					new FakeItem { IntValue = 3, DoubleValue = 3 }
				};
		}

		[Test]
		public void WhenRequestContainsNoSystemParametersThenReturnedModelFilterWithoutFiltering()
		{
			var filter = GetModelFilter(new NameValueCollection());
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(3, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsFilterParameterAndSortThenReturnedModelFilterFilteringAndSortedByValue()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$filter", "IntValue ge 1" }, { "$orderby", "IntValue desc" } });
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(3, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(1, filteredItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		public void WhenRequestContainsFilterSortSkipAndTopThenReturnedModelFilterFilteringFindsItem()
		{
			var filter = GetModelFilter(new NameValueCollection
				{
					{ "$filter", "IntValue ge 1" },
					{ "$skip", "1" },
					{ "$top", "1" },
					{ "$orderby", "IntValue desc" }
				});
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(2, filteredItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsFilterParameterThenReturnedModelFilterFilteringByValue()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$filter", "IntValue eq 1" } });
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsFilterParameterAndApplyingAsExtensionMethodThenReturnedModelFilterFilteringByValue()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$filter", "IntValue eq 1" } });
			var filteredItems = _items.Filter(filter);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsMathFunctionFilterParameterThenReturnedModelFilterFilteringByValue()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$filter", "floor(DoubleValue) gt 1" } });
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(2, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsSkipParameterThenReturnedModelFilterSkippingItems()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$skip", "2" } });
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(1, filteredItems.Count());
		}

		[Test]
		public void WhenRequestContainsTopParameterThenReturnedModelFilterWithTopItems()
		{
			var filter = GetModelFilter(new NameValueCollection { { "$top", "1" } });
			var filteredItems = filter.Filter(_items);

			Assert.AreEqual(1, filteredItems.Count());
		}

		private ModelFilter<FakeItem> GetModelFilter(NameValueCollection parameters)
		{
			var filter = _parser.Parse<FakeItem>(parameters);
			return filter;
		}
	}
}
