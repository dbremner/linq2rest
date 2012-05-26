// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Linq2Rest.Reactive.WinRT.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2Rest.Reactive.WinRT.Tests
{
	[TestClass]
	public class RestObservableTests
	{
		[TestMethod]
		public void CanCreateQbservable()
		{
			try
			{
				new RestObservable<FakeItem>(
								   new FakeAsyncRestClientFactory(),
								   new TestSerializerFactory());
			}
			catch
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void CanCreateSubscription()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());

			observable
				.SubscribeOn(Scheduler.ThreadPool)
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.ThreadPool)
				.Subscribe(
						   x =>
						   {
							   waitHandle.Set();
						   },
						   () =>
						   {
							   waitHandle.Set();
						   });

			var result = waitHandle.WaitOne();

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void WhenDisposingSubscriptionThenDoesNotExecute()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(2000), new TestSerializerFactory());
			var subscription = observable
				.SubscribeOn(Scheduler.TaskPool)
				.Where(x => x.StringValue == "blah")
				.ObserveOn(Scheduler.CurrentThread)
				.Subscribe(x => { }, () => waitHandle.Set());

			subscription.Dispose();
			var result = waitHandle.WaitOne(2000);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void WhenGroupingSourceThenReturnsResults()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			observable
						.Where(x => x.StringValue == "blah")
						.GroupBy(x => x.StringValue)
						.Subscribe(x => { }, () => waitHandle.Set());

			var result = waitHandle.WaitOne();

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void WhenGettingSingleThenReturnsResults()
		{
			var observable = new RestObservable<FakeItem>(new FakeAsyncRestClientFactory(), new TestSerializerFactory());
			var result = observable
				.Where(x => x.StringValue == "blah")
				.SingleOrDefault();

			Assert.IsNull(result);
		}
	}
}