namespace UrlQueryParser.Tests
{
	using System;
	using System.Linq;
	using Linq2Rest;
	using Linq2Rest.Parser;
	using Linq2Rest.Tests;
	using NUnit.Framework;

	public class SelectExpressionFactoryTests
	{
		private FakeItem[] _items;
		private SelectExpressionFactory<FakeItem> _factory;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			var memberNameResolver = new MemberNameResolver();
			_factory = new SelectExpressionFactory<FakeItem>(memberNameResolver, new RuntimeTypeProvider(memberNameResolver));

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
			var expression = _factory.Create("Number").Compile();

			var selection = _items.Select(expression);

			Assert.True(selection.All(x => x.GetType().GetProperty("Number") != null));
		}
	}
}