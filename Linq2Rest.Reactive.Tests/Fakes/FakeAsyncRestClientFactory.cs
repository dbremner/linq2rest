// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeAsyncRestClientFactory.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the FakeAsyncRestClientFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

		public void SetMethod(HttpMethod method)
		{
		}

		public void SetInput(Stream input)
		{
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

			public Task<Stream> Download()
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