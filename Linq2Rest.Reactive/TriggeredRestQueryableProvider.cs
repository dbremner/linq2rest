// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
namespace Linq2Rest.Reactive
{
	using System;
	using System.Linq.Expressions;
	using System.Reactive;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class TriggeredRestQueryableProvider : RestQueryableProviderBase
	{
		private readonly IObservable<Unit> _trigger;

		public TriggeredRestQueryableProvider(
			IObservable<Unit> trigger,
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
			_trigger = trigger;
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new TriggeredRestObservable<TResult>(_trigger, AsyncRestClient, SerializerFactory, expression, subscriberScheduler, observerScheduler);
		}
	}
}