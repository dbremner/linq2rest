// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
#if !WINDOWS_PHONE
	#endif
	using System;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RestQueryableProvider : RestQueryableProviderBase
	{
		public RestQueryableProvider(IAsyncRestClientFactory asyncRestClient, ISerializerFactory serializerFactory, IScheduler subscriberScheduler, IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new InnerRestObservable<TResult>(AsyncRestClient, SerializerFactory, expression, subscriberScheduler, observerScheduler);
		}
	}
}