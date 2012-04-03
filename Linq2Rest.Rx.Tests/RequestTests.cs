// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Linq.Expressions;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Reactive;
	using Linq2Rest.Reactive.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RequestTests
	{
		private Mock<IAsyncRestClient> _mockRestClient;
		private Mock<IAsyncRestClientFactory> _mockClientFactory;
		private RestObservable<FakeItem> _observable;

		[SetUp]
		public void Setup()
		{
			var mockResult = new Mock<IAsyncResult>();
			mockResult.SetupGet(x => x.CompletedSynchronously).Returns(true);
			_mockRestClient = new Mock<IAsyncRestClient>();
			_mockRestClient.Setup(x => x.BeginGetResult(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(mockResult.Object);
			_mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[]");

			_mockClientFactory = new Mock<IAsyncRestClientFactory>();
			_mockClientFactory.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClientFactory.Setup(x => x.Create(It.IsAny<Uri>())).Returns<Uri>(x => _mockRestClient.Object);

			_observable = new RestObservable<FakeItem>(_mockClientFactory.Object, new TestSerializerFactory());
		}

		[Test]
		public void WhenMainExpressionIsContainedInIsTrueExpressionThenUsesOperandExpression()
		{
			var waitHandle = new ManualResetEvent(false);

			var parameter = Expression.Parameter(typeof(FakeItem), "x");
			var trueExpression =
				Expression.IsTrue(
				Expression.LessThanOrEqual(Expression.Property(parameter, "IntValue"), Expression.Constant(3)));

			_observable
				.Where(Expression.Lambda<Func<FakeItem, bool>>(trueExpression, parameter))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var uri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithNoFilterThenCallsRestServiceOnce()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var uri = new Uri("http://localhost/");
			_mockClientFactory.Verify(x => x.Create(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryThenCallsRestServiceOnce()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.IntValue <= 3)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			_mockClientFactory.Verify(x => x.Create(It.IsAny<Uri>()), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithMultipleFiltersThenCallsRestServiceWithSingleFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.IntValue <= 3)
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var uri = new Uri("http://localhost/?$filter=(IntValue+le+3)+and+(StringValue+eq+'blah')");
			_mockClientFactory.Verify(x => x.Create(uri), Times.Once());
		}

		[Test]
		public void WhenGroupByExpressionRequiresEagerEvaluationThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.IntValue <= 3)
				.GroupBy(x => x.StringValue)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var uri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(uri), Times.Once());
		}

		[Test]
		public void WhenAnyExpressionRequiresEagerEvaluationThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.IntValue <= 3)
				.Any(x => x.DoubleValue.Equals(3d))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();


			var uri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(uri), Times.Once());
		}
	}
}
