// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestPutQueryProviderTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestPutQueryProviderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.IO;
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using Linq2Rest.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestPutQueryProviderTests
	{
		private RestPutQueryProvider<FakeItem> _provider;
		private Mock<IRestClient> _mockClient;
		private Stream _inputData;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_inputData = "[]".ToStream();
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClient.Setup(x => x.Put(It.IsAny<Uri>(), It.IsAny<Stream>())).Returns("[]".ToStream());
			_provider = new RestPutQueryProvider<FakeItem>(_mockClient.Object, new TestSerializerFactory(), new ExpressionProcessor(new ExpressionWriter()), _inputData);
		}

		[Test]
		public void WhenExecutingQueryThenPutsToRestClient()
		{
			Expression<Func<FakeItem, bool>> expression = x => true;
			_provider.Execute(expression);

			_mockClient.Verify(x => x.Put(It.IsAny<Uri>(), _inputData));
		}
	}
}