namespace Linq2Rest.Rx
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