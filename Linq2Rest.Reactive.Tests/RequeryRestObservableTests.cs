// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Linq2Rest.Reactive.Tests.Fakes;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class RequeryRestObservableTests
	{
		[Test]
		public void WhenObservablePollsThenDoesNotComplete()
		{
			var waitHandle = new ManualResetEvent(false);
			var factory = new FakeAsyncRestClientFactory("[{\"Text\":\"blah\", \"Number\":1}]");
			var observable = new RestObservable<FakeItem>(factory, new TestSerializerFactory());
			var subscription = observable
				.Requery(TimeSpan.FromSeconds(0.5))
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
				.Requery(TimeSpan.FromSeconds(0.5))
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

		[Test]
		public void WhenInvokingThenCallsRestClient()
		{
			var waitHandle = new ManualResetEvent(false);
			int i = 1;
			var mockResult = new Mock<IAsyncResult>();
			mockResult.SetupGet(x => x.CompletedSynchronously).Returns(true);
			var mockRestClient = new Mock<IAsyncRestClient>();
			mockRestClient.Setup(x => x.BeginGetResult(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
				.Callback<AsyncCallback, object>((a, o) => a.Invoke(mockResult.Object))
				.Returns(mockResult.Object);
			mockRestClient.Setup(x => x.EndGetResult(It.IsAny<IAsyncResult>())).Returns("[]".ToStream());

			var mockClientFactory = new Mock<IAsyncRestClientFactory>();
			mockClientFactory.SetupGet(x => x.ServiceBase).Returns(new Uri("http://localhost"));
			mockClientFactory.Setup(x => x.Create(It.IsAny<Uri>()))
				.Callback<Uri>(Console.WriteLine)
				.Returns(mockRestClient.Object);

			new RestObservable<FakeItem>(mockClientFactory.Object, new TestSerializerFactory())
				.Requery(TimeSpan.FromSeconds(1))
				.Where(x => x.IntValue == Interlocked.Increment(ref i))
				.Subscribe(x => { });

			waitHandle.WaitOne(5000);

			mockClientFactory.Verify(x => x.Create(It.IsAny<Uri>()), Times.AtLeast(2));
			mockClientFactory.Verify(x => x.Create(It.Is<Uri>(y => y.ToString() == "http://localhost/?$filter=IntValue+eq+2")));
			mockClientFactory.Verify(x => x.Create(It.Is<Uri>(y => y.ToString() == "http://localhost/?$filter=IntValue+eq+3")));
		}
	}
}