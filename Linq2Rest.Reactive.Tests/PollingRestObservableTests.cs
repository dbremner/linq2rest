namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Linq2Rest.Reactive.Tests.Fakes;
	using NUnit.Framework;

	[TestFixture]
	public class PollingRestObservableTests
	{
		[Test]
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

			Assert.False(result);
		}

		[Test]
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

			Assert.True(result);
		}
	}
}