// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
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
			var mockClient = new Mock<IRestClient>();
			mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_provider = new RestQueryProvider<FakeItem>(mockClient.Object, new TestSerializerFactory());
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

		[Test]
		public void WhenCreatingGenericQueryWithNullExpressionThenThrows()
		{
			Assert.Throws<ArgumentNullException>(() => _provider.CreateQuery<FakeItem>(null));
		}

		[Test]
		public void WhenCreatingNonGenericQueryWithNullExpressionThenThrows()
		{
			Assert.Throws<ArgumentNullException>(() => _provider.CreateQuery(null));
		}
	}
}