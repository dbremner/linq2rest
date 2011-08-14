// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Tests.Provider
{
	using System.Linq.Expressions;
	using System.Web.Script.Serialization;

	using Moq;

	using NUnit.Framework;

	using UrlQueryParser.Provider;

	public class RestQueryProviderTests
	{
		private RestQueryProvider<FakeItem> _provider;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_provider = new RestQueryProvider<FakeItem>(new Mock<IRestClient>().Object, new JavaScriptSerializer());
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