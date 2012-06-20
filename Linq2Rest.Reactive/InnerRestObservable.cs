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

	/// <summary>
	/// Defines an observable REST query.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object returned by the REST service.</typeparam>
	internal class InnerRestObservable<T> : InnerRestObservableBase<T>
	{
		private readonly RestQueryableProvider _provider;

		internal InnerRestObservable(
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(restClient, serializerFactory, expression, subscriberScheduler, observerScheduler)
		{
#if !WINDOWS_PHONE
			Contract.Requires(restClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);
#endif

			_provider = new RestQueryableProvider(restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

#if !WINDOWS_PHONE
		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_provider != null);
		}
#endif
	}
}