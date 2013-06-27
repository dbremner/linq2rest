// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestDeleteQueryProviderTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestDeleteQueryProviderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Linq.Expressions;
	using Fakes;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestDeleteQueryProviderTests
	{
		private RestDeleteQueryProvider<FakeItem> _provider;
		private Mock<IRestClient> _mockClient;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClient.Setup(x => x.Delete(It.IsAny<Uri>())).Returns("[]".ToStream());
			_provider = new RestDeleteQueryProvider<FakeItem>(_mockClient.Object, new TestSerializerFactory(), new ExpressionProcessor(new ExpressionWriter()));
		}

		[Test]
		public void WhenExecutingQueryThenDeletesToRestClient()
		{
			Expression<Func<FakeItem, bool>> expression = x => true;
			_provider.Execute(expression);

			_mockClient.Verify(x => x.Delete(It.IsAny<Uri>()));
		}
	}
}