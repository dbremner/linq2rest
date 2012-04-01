// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx
{
	using System.Linq.Expressions;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RestQueryableProvider : IQbservableProvider
	{
		private readonly IAsyncRestClientFactory _asyncRestClient;
		private readonly ISerializerFactory _serializerFactory;

		public RestQueryableProvider(IAsyncRestClientFactory asyncRestClient, ISerializerFactory serializerFactory)
		{
			_asyncRestClient = asyncRestClient;
			_serializerFactory = serializerFactory;
		}

		public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
		{
			return new RestObservable<TResult>(_asyncRestClient, _serializerFactory, expression);
		}
	}
}