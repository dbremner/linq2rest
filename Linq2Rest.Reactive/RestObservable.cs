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
		/// Creates an observable
		/// </summary>
		/// <returns></returns>
		public IQbservable<T> Create()
		{
			return new InnerRestObservable<T>(_restClientFactory, _serializerFactory, null, Scheduler.CurrentThread, Scheduler.CurrentThread);
		}

		public IQbservable<T> Poll(TimeSpan frequency)
		{
			return new PollingRestObservable<T>(frequency, _restClientFactory, _serializerFactory, null, Scheduler.CurrentThread, Scheduler.CurrentThread);
		}
	}
}
