// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Reactive.SL.IntegrationTests.Fakes;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class RestObservableTests
	{
		[TestMethod]
		public void CanSetUpObservable()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<SampleDto>(new FakeAsyncRestClientFactory(), new PhoneSerializerFactory());
			
			// Note: Reported erroneous ReSharper error message to JetBrains.
			var subscription = observable
				.Create()
				.Where(x => x.Text != "blah")
				.Subscribe(new TestObserver<SampleDto>(waitHandle));

			var result = waitHandle.WaitOne(5000);

			Assert.IsTrue(result);
		}
		
		[TestMethod]
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

			Assert.IsTrue(result);
		}

		[TestMethod]
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

			Assert.IsFalse(next);
			Assert.IsTrue(completed);
		}

		[TestMethod]
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

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void WhenResultReturnedThenCompletesSubscription()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			var subscription = observable
				.Create()
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => { }, () => waitHandle.Set());

			var result = waitHandle.WaitOne();

			Assert.IsTrue(result);
		}

		private class TestObserver<T> : IObserver<T>
		{
			private readonly ManualResetEvent _waitHandle;

			public TestObserver(ManualResetEvent waitHandle)
			{
				_waitHandle = waitHandle;
			}

			public void OnNext(T value)
			{
			}

			public void OnError(Exception error)
			{
			}

			public void OnCompleted()
			{
				_waitHandle.Set();
			}
		}
	}
}