// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Reactive;

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

		internal PollingRestObservable(
			TimeSpan frequency,
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(restClient, serializerFactory, expression, subscriberScheduler, observerScheduler)
		{
			Frequency = frequency;
			_provider = new PollingRestQueryableProvider(frequency, restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

		protected TimeSpan Frequency { get; private set; }

		protected override IObservable<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = RestClient.Create(builder.GetFullUri());

			return Observable.Interval(Frequency)
				.Select(x => Observable.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult).Invoke())
				.Select(x => x.Select(s => ReadIntermediateResponse(type, s)))
				.SelectMany(x => x);
		}

		protected override IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
			var serializer = SerializerFactory.Create<T>();

			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);

			return Observable.Interval(Frequency)
				.Select(x => Observable.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)())
				.Select(x => x.Select(serializer.DeserializeList))
				.SelectMany(x => x);
		}
	}

	internal class TriggeredRestObservable<T> : InnerRestObservableBase<T>
	{
		private readonly IObservable<Unit> _trigger;
		private readonly IQbservableProvider _provider;
		private IDisposable _internalSubscription;
		private IDisposable _subscribeSubscription;

		internal TriggeredRestObservable(
			IObservable<Unit> trigger,
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(restClient, serializerFactory, expression, subscriberScheduler, observerScheduler)
		{
			_trigger = trigger;
			_provider = new TriggeredRestQueryableProvider(trigger, restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

		public override IDisposable Subscribe(IObserver<T> observer)
		{
			Observers.Add(observer);
			_subscribeSubscription = SubscriberScheduler
				.Schedule(
						  observer,
						  (s, o) =>
						  {
							  _internalSubscription = _trigger
								  .Select(
								  x =>
								  {
									  var filter = Expression as MethodCallExpression;
									  var parameterBuilder = new ParameterBuilder(RestClient.ServiceBase);

									  return Processor.ProcessMethodCall(
										  filter,
										  parameterBuilder,
										  GetResults,
										  GetIntermediateResults);
								  })
									  .SelectMany(y => y)
									  .Subscribe(new ObserverPublisher(Observers, ObserverScheduler));
						  });
			return new RestSubscription(observer, Unsubscribe);
		}

		private void Unsubscribe(IObserver<T> observer)
		{
			if (!Observers.Contains(observer))
			{
				return;
			}

			Observers.Remove(observer);
			observer.OnCompleted();
			if (Observers.Count == 0)
			{
				if (_internalSubscription != null)
				{
					_internalSubscription.Dispose();
				}

				if (_subscribeSubscription != null)
				{
					_subscribeSubscription.Dispose();
				}
			}
		}
	}
}