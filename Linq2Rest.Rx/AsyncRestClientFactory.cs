// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;

	internal class AsyncRestClientFactory : IAsyncRestClientFactory
	{
		public AsyncRestClientFactory(Uri uri)
		{
			ServiceBase = uri;
		}

		public Uri ServiceBase { get; private set; }

		public IAsyncRestClient Create(Uri source)
		{
			return new AsyncRestClient(source);
		}
	}
}