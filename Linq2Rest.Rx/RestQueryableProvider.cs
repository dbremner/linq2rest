// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RestQueryableProvider : IQbservableProvider
	{
		private readonly IAsyncRestClientFactory _asyncRestClient;
		private readonly ISerializerFactory _serializerFactory;
		private readonly IScheduler _subscriberScheduler;
		private readonly IScheduler _observerScheduler;

		public RestQueryableProvider(
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
		{
			_asyncRestClient = asyncRestClient;
			_serializerFactory = serializerFactory;
			_subscriberScheduler = subscriberScheduler;
			_observerScheduler = observerScheduler;
		}

		public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
		{
			var methodCallExpression = expression as MethodCallExpression;
			if (methodCallExpression != null)
			{
				switch (methodCallExpression.Method.Name)
				{
					case "SubscribeOn":
						var subscribeScheduler = (methodCallExpression.Arguments[1] as ConstantExpression).Value as IScheduler;
						return new RestObservable<TResult>(
							_asyncRestClient,
							_serializerFactory,
							methodCallExpression.Arguments[0],
							subscribeScheduler,
							_observerScheduler);
					case "ObserveOn":
						var observeScheduler = (methodCallExpression.Arguments[1] as ConstantExpression).Value as IScheduler;
						return new RestObservable<TResult>(
							_asyncRestClient,
							_serializerFactory,
							methodCallExpression.Arguments[0],
							_subscriberScheduler,
							observeScheduler);
				}
			}
			return new RestObservable<TResult>(_asyncRestClient, _serializerFactory, expression, _subscriberScheduler, _observerScheduler);
		}
	}
}