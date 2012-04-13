// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void CanSetUpObservable()
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