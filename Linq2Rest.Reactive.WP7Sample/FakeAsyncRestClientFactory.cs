// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.WP7Sample
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Linq2Rest.Reactive;

	public class FakeAsyncRestClientFactory : IAsyncRestClientFactory
	{
		private readonly int _responseDelay;

		public FakeAsyncRestClientFactory()
			: this(-1)
		{

		}

		public FakeAsyncRestClientFactory(int responseDelay)
		{
			_responseDelay = responseDelay;
		}

		public Uri ServiceBase
		{
			get
			{
				return new Uri("http://localhost");
			}
		}

		public IAsyncRestClient Create(string source)
		{
			return new FakeAsyncResultClient(_responseDelay);
		}

		private class FakeAsyncResultClient : IAsyncRestClient
		{
			private readonly int _responseDelay;

			public FakeAsyncResultClient(int responseDelay)
			{
				_responseDelay = responseDelay;
			}

			public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
			{
				return new FakeAsyncResult(callback);
			}

			public string EndGetResult(IAsyncResult result)
			{
				if (_responseDelay > 0)
				{
					Thread.Sleep(_responseDelay);
				}

				return "[]";
			}

			private class FakeAsyncResult : IAsyncResult
			{
				public FakeAsyncResult(AsyncCallback callback)
				{
					Task.Factory.StartNew(() => callback.Invoke(this));
				}

				public bool IsCompleted
				{
					get
					{
						return true;
					}
				}
				public WaitHandle AsyncWaitHandle
				{
					get
					{
						return null;
					}
				}
				public object AsyncState
				{
					get
					{
						return null;
					}
				}
				public bool CompletedSynchronously
				{
					get
					{
						return true;
					}
				}
			}
		}
	}
}