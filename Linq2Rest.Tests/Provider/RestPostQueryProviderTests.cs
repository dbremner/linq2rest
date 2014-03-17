// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestPostQueryProviderTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestPostQueryProviderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.IO;
	using System.Linq.Expressions;
	using Linq2Rest.Parser;
	using Linq2Rest.Provider;
	using Linq2Rest.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestPostQueryProviderTests
	{
		private RestPostQueryProvider<FakeItem> _provider;
		private Mock<IRestClient> _mockClient;
		private Stream _inputData;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_inputData = "[]".ToStream();
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClient.Setup(x => x.Post(It.IsAny<Uri>(), It.IsAny<Stream>())).Returns("[]".ToStream());
			var memberNameResolver = new MemberNameResolver();
			_provider = new RestPostQueryProvider<FakeItem>(_mockClient.Object, new TestSerializerFactory(), new ExpressionProcessor(new ExpressionWriter(memberNameResolver), memberNameResolver), _inputData);
		}

		[Test]
		public void WhenExecutingQueryThenPostsToRestClient()
		{
			Expression<Func<FakeItem, bool>> expression = x => true;
			_provider.Execute(expression);

			_mockClient.Verify(x => x.Post(It.IsAny<Uri>(), _inputData));
		}
	}
}