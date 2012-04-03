// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.IO;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;

	internal class AsyncRestClient : IAsyncRestClient
	{
		private readonly Uri _source;

		public AsyncRestClient(Uri source)
		{
			_source = source;
		}

		public IAsyncResult BeginGetResult(AsyncCallback callback, object state)
		{
			return new RestAsyncResult(_source, callback, state);
		}

		public string EndGetResult(IAsyncResult result)
		{
			var restResult = (RestAsyncResult)result;

			return restResult.Response;
		}

		private class RestAsyncResult : IAsyncResult
		{
			private readonly AsyncCallback _callback;
			private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);

			public RestAsyncResult(Uri source, AsyncCallback callback, object state)
			{
				_callback = callback;
				AsyncState = state;
				var request = WebRequest.Create(source);
				Task.Factory
					.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, state)
					.ContinueWith(GetResponse);
			}

			private void GetResponse(Task<WebResponse> t)
			{
				if (t.Status == TaskStatus.RanToCompletion && t.Result != null)
				{
					var responseStream = t.Result.GetResponseStream();
					if (responseStream != null)
					{
						var reader = new StreamReader(responseStream);
						Response = reader.ReadToEnd();
					}
				}

				IsCompleted = true;
				_waitHandle.Set();
				_callback.Invoke(this);
			}

			public string Response { get; private set; }

			public bool IsCompleted { get; private set; }

			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return _waitHandle;
				}
			}

			public object AsyncState { get; private set; }

			public bool CompletedSynchronously
			{
				get
				{
					return false;
				}
			}
		}
	}
}