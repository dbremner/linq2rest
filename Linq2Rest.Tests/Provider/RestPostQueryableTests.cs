// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestPostQueryableTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestPostQueryableTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Linq.Expressions;
	using Fakes;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestPostQueryableTests
	{
		private RestPostQueryable<FakeItem> _postQueryable;
		private Mock<IRestClient> _mockClient;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			Expression<Func<FakeItem, bool>> expression = x => true;
			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClient.Setup(x => x.Post(It.IsAny<Uri>(), It.IsAny<Stream>())).Returns("[]".ToStream());
			_postQueryable = new RestPostQueryable<FakeItem>(_mockClient.Object, new TestSerializerFactory(), expression, "[]".ToStream());
		}

		[Test]
		public void ElementTypeIsSameAsGenericParameter()
		{
			Assert.AreEqual(typeof(FakeItem), _postQueryable.ElementType);
		}

		[Test]
		public void WhenDisposingThenDisposesClient()
		{
			_postQueryable.Dispose();

			_mockClient.Verify(x => x.Dispose());
		}

		[Test]
		public void WhenPosttingNonGenericEnumeratorThenDoesNotReturnNull()
		{
			Assert.NotNull((_postQueryable as IEnumerable).GetEnumerator());
		}
	}
}