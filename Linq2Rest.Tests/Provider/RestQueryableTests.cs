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

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			var mockClient = new Mock<IRestClient>();
			mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_queryable = new RestQueryable<FakeItem>(mockClient.Object, new TestSerializerFactory());
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
	}
}
