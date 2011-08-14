namespace UrlQueryParser.Tests
{
	using System.Linq;

	using NUnit.Framework;

	public class SelectExpressionFactoryTests
	{
		private FakeItem[] _items;
		private SelectExpressionFactory<FakeItem> _factory;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_factory = new SelectExpressionFactory<FakeItem>();

			_items = new[]
				{
					new FakeItem { IntValue = 2, DoubleValue = 5},
					new FakeItem { IntValue = 1, DoubleValue = 4},
					new FakeItem { IntValue = 3, DoubleValue = 4}
				};
		}

		[Test]
		public void WhenApplyingSelectionThenReturnsObjectWithOnlySelectedPropertiesAsFields()
		{
			var expression = _factory.Create("IntValue");

			var selection = _items.Select(expression);

			Assert.True(selection.All(x => x.GetType().GetField("IntValue") != null));
		}
	}
}