namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Reactive.Linq;
	using System.Threading;

	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void TestMethod1()
		{
			var waitHandle = new ManualResetEvent(false);
			var observable = new RestObservable<SampleDto>(new FakeAsyncRestClientFactory(), new PhoneSerializerFactory());
			var subscription = observable
				.Where(x => x.Text != "blah")
				.Subscribe(new TestObserver<SampleDto>(waitHandle));

			var result = waitHandle.WaitOne(5000);

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