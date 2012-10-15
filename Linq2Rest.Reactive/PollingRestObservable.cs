// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.IO;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Reactive.Threading.Tasks;
	using System.Threading.Tasks;
	using Linq2Rest.Provider;

	internal class PollingRestObservable<T> : InnerRestObservableBase<T>
	{
		private readonly IQbservableProvider _provider;

		internal PollingRestObservable(
			TimeSpan frequency,
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

			Frequency = frequency;
			_provider = new PollingRestQueryableProvider(frequency, restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}

		protected TimeSpan Frequency { get; private set; }

		protected override IObservable<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = RestClient.Create(builder.GetFullUri());
			;
			return Observable.Interval(Frequency)
				.Select(x => Observable.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)())
				.Select(x => x.Select(s => ReadIntermediateResponse(type, s)))
				.SelectMany(x => x);
		}

		protected override IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
			var serializer = SerializerFactory.Create<T>();

			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);

			return Observable.Interval(Frequency)
				.Select(x => Observable.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)())
				.Select(x => x.Select(serializer.DeserializeList))
				.SelectMany(x => x);
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