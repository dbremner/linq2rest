namespace Linq2Rest.Rx
{
	using System;

	/// <summary>
	/// Defines the public interface for the async REST client factory.
	/// </summary>
	public interface IAsyncRestClientFactory
	{
		/// <summary>
		/// Gets the base service address.
		/// </summary>
		Uri ServiceBase { get; }

		/// <summary>
		/// Creates an <see cref="IAsyncRestClient"/>.
		/// </summary>
		/// <param name="source">The <see cref="Uri"/> to download from.</param>
		/// <returns>An <see cref="IAsyncRestClient"/> instance.</returns>
		IAsyncRestClient Create(Uri source);
	}
}