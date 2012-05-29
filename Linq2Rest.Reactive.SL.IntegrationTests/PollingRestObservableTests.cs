// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Linq2Rest.Reactive.SL.IntegrationTests.Fakes;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class PollingRestObservableTests
	{
		[TestMethod]
		public void WhenObservablePollsThenDoesNotComplete()
		{
			var waitHandle = new ManualResetEvent(false);
			var factory = new FakeAsyncRestClientFactory("[{\"Text\":\"blah\", \"Number\":1}]");
			var observable = new RestObservable<FakeItem>(factory, new TestSerializerFactory());
			var subscription = observable
				.Poll(TimeSpan.FromSeconds(0.5))
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => { }, () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void WhenDisposingPollSubscriptionThenCompletes()
		{
			var waitHandle = new ManualResetEvent(false);
			var factory = new FakeAsyncRestClientFactory("[{\"Text\":\"blah\", \"Number\":1}]");
			var observable = new RestObservable<FakeItem>(factory, new TestSerializerFactory());
			var subscription = observable
				.Poll(TimeSpan.FromSeconds(0.5))
				.Where(x => x.StringValue == "blah")
				.Subscribe(x => { }, () => waitHandle.Set());

			Task.Factory.StartNew(
								  () =>
								  {
									  Thread.Sleep(1000);
									  subscription.Dispose();
								  });

			var result = waitHandle.WaitOne();

			Assert.IsTrue(result);
		}
	}
}