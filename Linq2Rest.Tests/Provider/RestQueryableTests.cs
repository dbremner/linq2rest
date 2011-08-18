// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using Linq2Rest.Provider;

	using Moq;

	using NUnit.Framework;

	public class RestQueryableTests
	{
		[Test]
		public void ElementTypeIsSameAsGenericParameter()
		{
			var queryable = new RestQueryable<FakeItem>(new Mock<IRestClient>().Object, new TestSerializerFactory());

			Assert.AreEqual(typeof(FakeItem), queryable.ElementType);
		}
	}
}
