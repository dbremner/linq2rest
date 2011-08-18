// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

	public class RestQueryProviderTests
	{
		private RestQueryProvider<FakeItem> _provider;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_provider = new RestQueryProvider<FakeItem>(new Mock<IRestClient>().Object, new TestSerializerFactory());
		}

		[Test]
		public void WhenCreatingNonGenericQueryThenReturnsQueryableWithPassedExpression()
		{
			Expression expression = Expression.New(typeof(FakeItem));

			var queryable = _provider.CreateQuery(expression);

			Assert.AreSame(expression, queryable.Expression);
		}

		[Test]
		public void WhenCreatingGenericQueryThenReturnsQueryableWithPassedExpression()
		{
			Expression expression = Expression.New(typeof(FakeItem));

			var queryable = _provider.CreateQuery<FakeItem>(expression);

			Assert.AreSame(expression, queryable.Expression);
		}
	}
}