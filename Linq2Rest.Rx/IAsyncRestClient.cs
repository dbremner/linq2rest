namespace Linq2Rest.Rx
{
	using System;

	public interface IAsyncRestClient
	{
		IAsyncResult BeginGetResult(AsyncCallback callback, object state);

		string EndGetResult(IAsyncResult result);
	}
}