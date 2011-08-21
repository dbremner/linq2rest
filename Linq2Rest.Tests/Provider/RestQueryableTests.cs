// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System.Collections;

	using Linq2Rest.Provider;

	using Moq;

	using NUnit.Framework;

	public class RestQueryableTests
	{
		private RestQueryable<FakeItem> _queryable;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_queryable = new RestQueryable<FakeItem>(new Mock<IRestClient>().Object, new TestSerializerFactory());
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
