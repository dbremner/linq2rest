namespace Linq2Rest.Tests.Parser
{
	using System.Linq;
	using Linq2Rest.Parser;
	using NUnit.Framework;

	[TestFixture]
	public class SelectExpressionFactoryTests
	{
		[Test]
		public void WhenCreatingSelectExpressionFromDataMemberOnFieldThenGetsFieldValue()
		{
			var factory = new SelectExpressionFactory<FakeItem>();
			var items = new[] { new FakeItem { StringValue = "test" } };

			var expression = factory.Create("Text");

			dynamic result = items.AsQueryable().Select(expression).First();

			Assert.AreEqual("test", result.Text);
		}
	}
}
