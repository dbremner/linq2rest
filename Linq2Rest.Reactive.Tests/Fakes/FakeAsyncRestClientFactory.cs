// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using Linq2Rest.Reactive;

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

			public Task<Stream> Get()
			{
				return CreateTask();
			}

			public Task<Stream> Post(Stream input)
			{
				return CreateTask();
			}

			public Task<Stream> Put(Stream input)
			{
				return CreateTask();
			}

			public Task<Stream> Delete()
			{
				return CreateTask();
			}

			private Task<Stream> CreateTask()
			{
				return Task.Factory.StartNew(() =>
					                             {
						                             if (_responseDelay > 0)
						                             {
							                             Thread.Sleep(_responseDelay);
						                             }

						                             return _response.ToStream();
					                             });
			}
		}
	}
}