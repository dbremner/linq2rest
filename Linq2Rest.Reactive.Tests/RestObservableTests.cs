// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.IO;
using System.Threading.Tasks;

namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Reactive;
	using Linq2Rest.Reactive.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RestObservableTests
	{
		[Test]
		public void CanCreateQbservable()
		{
			Assert.DoesNotThrow(
								() => new RestObservable<FakeItem>(
									new FakeAsyncRestClientFactory(),
									new TestSerializerFactory()));
		}

		[Test]
		public void CanCreateSubscription()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());

			observable
				.Create()
				.SubscribeOn(NewThreadScheduler.Default)
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.Default)
				.Subscribe(
						   x =>
						   {
							   Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
							   waitHandle.Set();
						   },
						   () =>
						   {
							   Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
							   waitHandle.Set();
						   });

			var result = waitHandle.WaitOne(5000);

			Assert.True(result);
		}

		[Test]
		public void WhenObservingOnDifferentSchedulerThenInvocationHappensOnDifferentThread()
		{
			var testThreadId = Thread.CurrentThread.ManagedThreadId;

			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			observable
				.Create()
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.Default)
				.Subscribe(
					x =>
					{
					},
					() =>
					{
						var observerThreadId = Thread.CurrentThread.ManagedThreadId;
						if (observerThreadId != testThreadId)
						{
							waitHandle.Set();
						}
					});

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenDisposingSubscriptionThenDoesNotExecute()
		{
			var completedWaitHandle = new ManualResetEvent(false);
			var onnextWaitHandle = new ManualResetEvent(false);

			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(2000), new TestSerializerFactory());
			var subscription = observable
				.Create()
				.SubscribeOn(Scheduler.CurrentThread)
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.CurrentThread)
				.Subscribe(x => onnextWaitHandle.Set(), () => completedWaitHandle.Set());

			subscription.Dispose();

			var next = onnextWaitHandle.WaitOne(2000);
			var completed = completedWaitHandle.WaitOne(2000);

			Assert.False(next);
			Assert.True(completed);
		}

		[Test]
		public void WhenGroupingSourceThenReturnsResults()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			observable
				.Create()
				.Where(x => x.StringValue == "blah")
				.GroupBy(x => x.StringValue)
				.Subscribe(x => { }, () => waitHandle.Set());

			var result = waitHandle.WaitOne();

			Assert.True(result);
		}

		[Test]
		public void WhenGettingSingleThenReturnsResults()
		{
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			var result = observable
				.Create()
				.Where(x => x.StringValue == "blah")
				.SingleOrDefault();

			Assert.Null(result);
		}

		[Test]
		public void WhenResultReturnedThenCompletesSubscription()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			var subscription = observable
				.Create()
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => { }, () => waitHandle.Set());

			var result = waitHandle.WaitOne();

			Assert.True(result);
		}

		[Test]
		public void WhenInvokingThenCallsRestClient()
		{
			var waitHandle = new ManualResetEvent(false);

			var mockRestClient = new Mock<IAsyncRestClient>();
			mockRestClient.Setup(x => x.Download())
				.Returns(() => Task<Stream>.Factory.StartNew(() => "[]".ToStream()));
			
			var mockClientFactory = new Mock<IAsyncRestClientFactory>();
			mockClientFactory.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			mockClientFactory.Setup(x => x.Create(It.IsAny<Uri>())).Returns(mockRestClient.Object);

			new RestObservable<FakeItem>(mockClientFactory.Object, new TestSerializerFactory())
				.Create()
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne(5000);

			mockRestClient.Verify(x => x.Download());
		}
	}
}