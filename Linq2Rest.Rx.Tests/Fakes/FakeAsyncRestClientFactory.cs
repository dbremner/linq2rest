// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx.Tests.Fakes
{
	using System;
	using System.Threading;
	using Linq2Rest.Reactive;

	public class FakeAsyncRestClientFactory : IAsyncRestClientFactory
	{
		public Uri ServiceBase
		{
			get
			{
				return new Uri("http://localhost");
			}
		}

		public IAsyncRestClient Create(Uri source)
		{
			return new FakeAsyncResultClient();
		}

		private class FakeAsyncResultClient : IAsyncRestClient
		{
			public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
			{
				return new FakeAsyncResult();
			}

			public string EndGetResult(IAsyncResult result)
			{
				return "[]";
			}

			private class FakeAsyncResult : IAsyncResult
			{
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