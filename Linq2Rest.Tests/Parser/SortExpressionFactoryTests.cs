// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Linq;
	using Linq2Rest.Parser;
	using NUnit.Framework;

	[TestFixture]
	public class SortExpressionFactoryTests
	{
		private FakeItem[] _items;
		private SortExpressionFactory _factory;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_factory = new SortExpressionFactory();
		}

		[SetUp]
		public void TestSetup()
		{
			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 5, StringValue = "aa" },
					new FakeItem { IntValue = 1, DoubleValue = 4, StringValue = "a" },
					new FakeItem { IntValue = 3, DoubleValue = 4, StringValue = "aaa" }
				};
		}

		[Test]
		public void WhenFilterIsEmptyThenDoesNotSort()
		{
			var descriptions = _factory.Create<FakeItem>(string.Empty);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);

			var sortedItems = filter.Filter(_items).ToArray();

			Assert.AreEqual(2, sortedItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, sortedItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(3, sortedItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		public void WhenFilterContainSortDescriptionWithoutDirectionThenCreatesMatchingAscendingSortDescription()
		{
			const string Orderstring = "IntValue";

			var descriptions = _factory.Create<FakeItem>(Orderstring);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);

			var sortedItems = filter.Filter(_items).ToArray();

			Assert.AreEqual(1, sortedItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(2, sortedItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(3, sortedItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		public void WhenFilterContainSortDescriptionWithDirectionThenCreatesMatchingSortDescription()
		{
			const string Orderstring = "IntValue desc";

			var descriptions = _factory.Create<FakeItem>(Orderstring);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);

			var sortedItems = filter.Filter(_items).ToArray();

			Assert.AreEqual(3, sortedItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(2, sortedItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(1, sortedItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		public void WhenFilterContainsSortMultipleDescriptionsThenSortsByAll()
		{
			const string Orderstring = "DoubleValue,IntValue desc";

			var descriptions = _factory.Create<FakeItem>(Orderstring);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);

			var sortedItems = filter.Filter(_items).ToArray();

			Assert.AreEqual(3, sortedItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, sortedItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(2, sortedItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}

		[Test]
		public void WhenOrderingByChildPropertyThenUsesChildProperty()
		{
			const string Orderstring = "StringValue/Length desc";

			var descriptions = _factory.Create<FakeItem>(Orderstring);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);
			var sortedItems = filter.Filter(_items).ToArray();

			Assert.AreEqual("aaa", sortedItems.OfType<FakeItem>().ElementAt(0).StringValue);
			Assert.AreEqual("aa", sortedItems.OfType<FakeItem>().ElementAt(1).StringValue);
			Assert.AreEqual("a", sortedItems.OfType<FakeItem>().ElementAt(2).StringValue);
		}
	}
}
