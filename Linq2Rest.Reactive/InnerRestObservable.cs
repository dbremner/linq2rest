namespace Linq2Rest.Reactive
{
	using System;
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
			_provider = new RestQueryableProvider(restClient, serializerFactory, subscriberScheduler, observerScheduler);
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public override IQbservableProvider Provider
		{
			get { return _provider; }
		}
	}
}