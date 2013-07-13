// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InnerRestObservable.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines an observable REST query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Provider;

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
			Contract.Requires(restClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);

			_provider = new RestQueryableProvider(restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_provider != null);
		}
	}
}