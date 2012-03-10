// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Collections;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestQueryableTests
	{
		private RestQueryable<FakeItem> _queryable;
		private Mock<IRestClient> _mockClient;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
		    _mockClient.Setup(x => x.Get(It.IsAny<Uri>())).Returns("[]");
			_queryable = new RestQueryable<FakeItem>(_mockClient.Object, new TestSerializerFactory());
		}

		[Test]
		public void ElementTypeIsSameAsGenericParameter()
		{
			Assert.AreEqual(typeof(FakeItem), _queryable.ElementType);
		}

		[Test]
		public void WhenGettingNonGenericEnumeratorThenDoesNotReturnNull()
		{
			Assert.NotNull((_queryable as IEnumerable).GetEnumerator());
		}

		[Test]
		public void WhenDisposingThenDisposesClient()
		{
			_queryable.Dispose();

			_mockClient.Verify(x => x.Dispose());
		}
	}
}
