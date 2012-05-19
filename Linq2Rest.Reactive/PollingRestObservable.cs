namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class PollingRestObservable<T> : InnerRestObservableBase<T>
	{
		private readonly IQbservableProvider _provider;
		private readonly TimeSpan _frequency;

		internal PollingRestObservable(
			TimeSpan frequency,
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(restClient, serializerFactory, expression, subscriberScheduler, observerScheduler)
		{
			_frequency = frequency;
			_provider = new PollingRestQueryableProvider(frequency, restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

		protected override IObservable<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = RestClient.Create(builder.GetFullUri());

			return Observable
				.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)
				.Invoke()
				.Select(x => ReadIntermediateResponse(type, x));
		}

		protected override IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);
			var serializer = SerializerFactory.Create<T>();
			return Observable.Interval(_frequency)
				.Select(x => Observable.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)())
				.Select(x => x.Select(serializer.DeserializeList))
				.SelectMany(x => x);
		}
	}
}