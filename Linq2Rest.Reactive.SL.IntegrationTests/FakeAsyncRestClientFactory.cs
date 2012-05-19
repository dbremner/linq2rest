// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;

	public class FakeAsyncRestClientFactory : IAsyncRestClientFactory
	{
		private readonly int _responseDelay;
		private readonly string _response = "[]";

		public FakeAsyncRestClientFactory()
			: this(-1)
		{
		}

		public FakeAsyncRestClientFactory(string response)
			: this(-1)
		{
			_response = response;
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

		public IAsyncRestClient Create(Uri source)
		{
			return new FakeAsyncResultClient(_responseDelay, _response);
		}

		private class FakeAsyncResultClient : IAsyncRestClient
		{
			private readonly int _responseDelay;
			private readonly string _response;

			public FakeAsyncResultClient(int responseDelay, string response)
			{
				_responseDelay = responseDelay;
				_response = response;
			}

			public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
			{
				return new FakeAsyncResult(callback);
			}

			public Stream EndGetResult(IAsyncResult result)
			{
				if (_responseDelay > 0)
				{
					Thread.Sleep(_responseDelay);
				}

				return _response.ToStream();
			}

			private class FakeAsyncResult : IAsyncResult
			{
				public FakeAsyncResult(AsyncCallback callback)
				{
					Task.Factory.StartNew(() => callback(this));
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
						return new ManualResetEvent(true);
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