// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RequeryingRestObservable<T> : InnerRestObservableBase<T>
	{
		private readonly TimeSpan _frequency;
		private readonly IQbservableProvider _provider;
		private IDisposable _subscribeSubscription;
		private IDisposable _internalSubscription;

		internal RequeryingRestObservable(
			TimeSpan frequency,
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(restClient, serializerFactory, expression, subscriberScheduler, observerScheduler)
		{
			_frequency = frequency;
			_provider = new RequeryingRestQueryableProvider(
				frequency,
				restClient,
				serializerFactory,
				subscriberScheduler,
				observerScheduler);
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
							  _internalSubscription = Observable.Interval(_frequency)
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