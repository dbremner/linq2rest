// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx.Tests
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Reactive;
	using Linq2Rest.Rx.Tests.Fakes;
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
				.SubscribeOn(Scheduler.NewThread)
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.ThreadPool)
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

			var result = waitHandle.WaitOne();

			Assert.True(result);
		}

		[Test]
		public void WhenObservingOnDifferentSchedulerThenInvocationHappensOnDifferentThread()
		{
			var testThreadId = Thread.CurrentThread.ManagedThreadId;

			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			observable
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.ThreadPool)
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
		public void WhenInvokingThenCallsRestClient()
		{
			var waitHandle = new ManualResetEvent(false);

			var mockResult = new Mock<IAsyncResult>();
			mockResult.SetupGet(x => x.CompletedSynchronously).Returns(true);
			var mockRestClient = new Mock<IAsyncRestClient>();
			mockRestClient.Setup(x => x.BeginGetResult(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(mockResult.Object);
			mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[]");

			var mockClientFactory = new Mock<IAsyncRestClientFactory>();
			mockClientFactory.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			mockClientFactory.Setup(x => x.Create(It.IsAny<Uri>())).Returns(mockRestClient.Object);

			new RestObservable<FakeItem>(mockClientFactory.Object, new TestSerializerFactory())
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			waitHandle.WaitOne();

			mockRestClient.Verify(x => x.BeginGetResult(It.IsAny<AsyncCallback>(), It.IsAny<object>()));
			mockRestClient.Verify(x => x.EndGetResult(It.IsAny<IAsyncResult>()));
		}
	}
}