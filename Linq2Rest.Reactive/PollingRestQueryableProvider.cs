namespace Linq2Rest.Reactive
{
	using System;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class PollingRestQueryableProvider : RestQueryableProviderBase
	{
		private readonly TimeSpan _frequency;

		public PollingRestQueryableProvider(
			TimeSpan frequency,
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
			_frequency = frequency;
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new PollingRestObservable<TResult>(_frequency, AsyncRestClient, SerializerFactory, expression, subscriberScheduler, observerScheduler);
		}
	}
}