// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggeredRestQueryableProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TriggeredRestQueryableProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using System.Reactive;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Provider;

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
			Contract.Requires(trigger != null);
			Contract.Requires(asyncRestClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);

			_trigger = trigger;
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new TriggeredRestObservable<TResult>(_trigger, AsyncRestClient, SerializerFactory, expression, subscriberScheduler, observerScheduler);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_trigger != null);
		}
	}
}