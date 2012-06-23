// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RequeryingRestQueryableProvider : RestQueryableProviderBase
	{
		private readonly TimeSpan _frequency;

		public RequeryingRestQueryableProvider(
			TimeSpan frequency,
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
#if !WINDOWS_PHONE
			Contract.Requires(asyncRestClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);
#endif

			_frequency = frequency;
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new RequeryingRestObservable<TResult>(
				_frequency,
				AsyncRestClient,
				SerializerFactory,
				expression,
				subscriberScheduler,
				observerScheduler);
		}
	}
}