// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggeredRestObservable.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TriggeredRestObservable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;
	using System.Threading;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using System.Reactive;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

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
			Contract.Requires(restClient != null);
			Contract.Requires(trigger != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);

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

		/// <summary>
		/// Notifies the provider that an observer is to receive notifications.
		/// </summary>
		/// <returns>
		/// A reference to an interface that allows observers to stop receiving notifications before the provider has finished sending them.
		/// </returns>
		/// <param name="observer">The object that is to receive notifications.</param>
		public override IDisposable Subscribe(IObserver<T> observer)
		{
			if (_internalSubscription != null)
			{
				_internalSubscription.Dispose();
			}

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
											IObservable<T> source = null;
											using (var waitHandle = new ManualResetEventSlim(false))
											{
												SubscriberScheduler.Schedule(() =>
																				 {
																					 try
																					 {
																						 source = Processor.ProcessMethodCall(
																							 filter,
																							 parameterBuilder,
																							 GetResults,
																							 GetIntermediateResults);
																					 }
																					 catch (Exception e)
																					 {
																						 source = Observable.Throw(e, default(T));
																					 }
																					 finally
																					 {
																						 waitHandle.Set();
																					 }
																				 });
												waitHandle.Wait();
											}

											return source;
										})
								.SelectMany(y => y)
								.Subscribe(new ObserverPublisher<T>(Observers, ObserverScheduler));
						});
			return new RestSubscription<T>(observer, Unsubscribe);
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

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_trigger != null);
			Contract.Invariant(_provider != null);
		}
	}
}