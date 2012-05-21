// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Reactive;

namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines an observable REST query.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object returned by the REST service.</typeparam>
	public class RestObservable<T>
	{
		private readonly IAsyncRestClientFactory _restClientFactory;
		private readonly ISerializerFactory _serializerFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestObservable{T}"/> class.
		/// </summary>
		/// <param name="restClientFactory">An <see cref="IAsyncRestClientFactory"/> to perform requests.</param>
		/// <param name="serializerFactory">An <see cref="ISerializerFactory"/> to perform deserialization.</param>
		public RestObservable(IAsyncRestClientFactory restClientFactory, ISerializerFactory serializerFactory)
		{
#if !WINDOWS_PHONE
			Contract.Requires<ArgumentNullException>(restClientFactory != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
#endif
			_restClientFactory = restClientFactory;
			_serializerFactory = serializerFactory;
		}

		/// <summary>
		/// Creates an observable performing a single call to the defined service.
		/// </summary>
		/// <returns>An instance of an <see cref="IQbservable{T}"/>.</returns>
		public IQbservable<T> Create()
		{
			return new InnerRestObservable<T>(
				_restClientFactory,
				_serializerFactory,
				null,
				Scheduler.CurrentThread,
				Scheduler.CurrentThread);
		}

		/// <summary>
		/// Creates an observable performing a continuous calls to the defined service at the defined intervals.
		/// </summary>
		/// <param name="interval">The interval between requests.</param>
		/// <returns>An instance of an <see cref="IQbservable{T}"/>.</returns>
		public IQbservable<T> Poll(TimeSpan interval)
		{
			return new PollingRestObservable<T>(
				interval,
				_restClientFactory,
				_serializerFactory,
				null,
				Scheduler.CurrentThread,
				Scheduler.CurrentThread);
		}

		/// <summary>
		/// Creates an observable performing calls when triggered.
		/// </summary>
		/// <param name="trigger">The trigger.</param>
		/// <returns>An instance of an <see cref="IQbservable{T}"/>.</returns>
		public IQbservable<T> Triggered(IObservable<Unit> trigger)
		{
			return new TriggeredRestObservable<T>(
				trigger,
				_restClientFactory,
				_serializerFactory,
				null,
				Scheduler.CurrentThread,
				Scheduler.CurrentThread);
		}

		/// <summary>
		/// Creates an observable performing a continuous calls to the defined service at the defined intervals.
		/// </summary>
		/// <param name="interval">The interval between requests.</param>
		/// <returns>An instance of an <see cref="IQbservable{T}"/>.</returns>
		public IQbservable<T> Requery(TimeSpan interval)
		{
			return new RequeryingRestObservable<T>(
				interval,
				_restClientFactory,
				_serializerFactory,
				null,
				Scheduler.CurrentThread,
				Scheduler.CurrentThread);
		}
	}
}
