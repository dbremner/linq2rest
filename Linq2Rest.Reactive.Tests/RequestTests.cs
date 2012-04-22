// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Linq;
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
			_mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[]".ToStream());

			_mockClientFactory = new Mock<IAsyncRestClientFactory>();
			_mockClientFactory.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			_mockClientFactory.Setup(x => x.Create(It.IsAny<Uri>())).Returns(_mockRestClient.Object);

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

			var requestUri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithNoFilterThenCallsRestServiceOnce()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var requestUri = new Uri("http://localhost/");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
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

			var requestUri = new Uri("http://localhost/?$filter=(IntValue+le+3)+and+(StringValue+eq+'blah')");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingAnyQueryThenCallsRestServiceWithFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.Children.Any(y => y.Text == "blah"))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne(2000);

			var requestUri = new Uri("http://localhost/?$filter=Children%2fany(y:+y%2fText+eq+'blah')");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingAllQueryThenCallsRestServiceWithFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.Children.All(y => y.Text == "blah"))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne(2000);

			var requestUri = new Uri("http://localhost/?$filter=Children%2fall(y:+y%2fText+eq+'blah')");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingNestedAllQueryThenCallsRestServiceWithFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Where(x => x.Children.All(y => y.Descendants.Any(z => z.Text == "blah")))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne(2000);

			var requestUri = new Uri("http://localhost/?$filter=Children%2fall(y:+y%2fDescendants%2fany(z:+z%2fText+eq+'blah'))");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
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

			var requestUri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingTakeFilterThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Take(1)

				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var requestUri = new Uri("http://localhost/?$top=1");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenQueryIncludesSideEffectsThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);
			var action = new Action<FakeItem>(x => { });

			_observable
				.Take(1)
				.Do(action)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var requestUri = new Uri("http://localhost/?$top=1");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenQueryIncludesSideEffectsThenInvokesSideEffect()
		{
			_mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[{\"DoubleValue\":1.2}]".ToStream());
			var waitHandle = new ManualResetEvent(false);
			var action = new Action<FakeItem>(x => waitHandle.Set());
			var mockObserver = new Mock<IObserver<FakeItem>>();

			_observable
				.Take(1)
				.Do(action)
				.Subscribe(mockObserver.Object);

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenQueryIncludesFinalEffectsThenInvokesSideEffect()
		{
			_mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[{\"DoubleValue\":1.2}]".ToStream());
			var waitHandle = new ManualResetEvent(false);
			var action = new Action(() => waitHandle.Set());
			var mockObserver = new Mock<IObserver<FakeItem>>();

			_observable
				.Take(1)
				.Finally(action)
				.Subscribe(mockObserver.Object);

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenApplyingSkipFilterThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Skip(1)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var requestUri = new Uri("http://localhost/?$skip=1");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}

		[Test]
		public void WhenApplyingProjectionThenCallsRestServiceWithExistingFilterParameter()
		{
			var waitHandle = new ManualResetEvent(false);

			_observable
				.Select(x => new { x.StringValue, x.IntValue })
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			var requestUri = new Uri("http://localhost/?$select=StringValue,IntValue");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
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

			var requestUri = new Uri("http://localhost/?$filter=IntValue+le+3");
			_mockClientFactory.Verify(x => x.Create(requestUri), Times.Once());
		}
	}
}
