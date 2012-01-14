// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Linq;

	using Linq2Rest.Parser;

	using NUnit.Framework;

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
					new FakeItem { IntValue = 2, DoubleValue = 5 },
					new FakeItem { IntValue = 1, DoubleValue = 4 },
					new FakeItem { IntValue = 3, DoubleValue = 4 }
				};
		}

		[Test]
		public void WhenFilterIsEmptyThenDoesNotSort()
		{
			var descriptions = _factory.Create<FakeItem>(string.Empty);
			var filter = new ModelFilter<FakeItem>(x => true, null, descriptions, 0, -1);

			var sortedItems = filter.Filter(_items);

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

			var sortedItems = filter.Filter(_items);

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

			var sortedItems = filter.Filter(_items);

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

			var sortedItems = filter.Filter(_items);

			Assert.AreEqual(3, sortedItems.OfType<FakeItem>().ElementAt(0).IntValue);
			Assert.AreEqual(1, sortedItems.OfType<FakeItem>().ElementAt(1).IntValue);
			Assert.AreEqual(2, sortedItems.OfType<FakeItem>().ElementAt(2).IntValue);
		}
	}
}
