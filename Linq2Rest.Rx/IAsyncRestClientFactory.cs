namespace Linq2Rest.Rx
{
	using System;

	public interface IAsyncRestClientFactory
	{
		Uri ServiceBase { get; }

		IAsyncRestClient Create(Uri source);
	}
}