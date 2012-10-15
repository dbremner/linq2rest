// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using Linq2Rest.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestGetQueryProviderTests
	{
		private RestGetQueryProvider<FakeItem> _provider;
		private Mock<IRestClient> _mockClient;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_provider = new RestGetQueryProvider<FakeItem>(_mockClient.Object, new TestSerializerFactory(), new ExpressionProcessor(new ExpressionWriter()));
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

		[Test]
		public void WhenDisposingThenDisposesClient()
		{
			_provider.Dispose();

			_mockClient.Verify(x => x.Dispose());
		}
	}
}