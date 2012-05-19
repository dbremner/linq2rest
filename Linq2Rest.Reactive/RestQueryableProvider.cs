// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
#if !WINDOWS_PHONE
	using System;
	using System.Diagnostics.Contracts;
#endif
	using System;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal abstract class RestQueryableProviderBase : IQbservableProvider
	{
		private readonly IAsyncRestClientFactory _asyncRestClient;
		private readonly ISerializerFactory _serializerFactory;
		private readonly IScheduler _subscriberScheduler;
		private readonly IScheduler _observerScheduler;

		public RestQueryableProviderBase(
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
		{
#if !WINDOWS_PHONE
			Contract.Requires(asyncRestClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);
#endif

			_asyncRestClient = asyncRestClient;
			_serializerFactory = serializerFactory;
			_subscriberScheduler = subscriberScheduler;
			_observerScheduler = observerScheduler;
		}

		protected IAsyncRestClientFactory AsyncRestClient
		{
			get { return _asyncRestClient; }
		}

		protected ISerializerFactory SerializerFactory
		{
			get { return _serializerFactory; }
		}

		public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
		{
			var methodCallExpression = expression as MethodCallExpression;
			if (methodCallExpression != null)
			{
				switch (methodCallExpression.Method.Name)
				{
					case "SubscribeOn":
						{
							var constantExpression = methodCallExpression.Arguments[1] as ConstantExpression;

#if !WINDOWS_PHONE
							Contract.Assume(constantExpression != null);
#endif

							var subscribeScheduler = constantExpression.Value as IScheduler;

#if !WINDOWS_PHONE
							Contract.Assume(subscribeScheduler != null);
#endif

							return CreateQbservable<TResult>(
								methodCallExpression.Arguments[0],
								subscribeScheduler,
								_observerScheduler);
						}

					case "ObserveOn":
						{
							var constantExpression = methodCallExpression.Arguments[1] as ConstantExpression;

#if !WINDOWS_PHONE
							Contract.Assume(constantExpression != null);
#endif

							var observeScheduler = constantExpression.Value as IScheduler;

#if !WINDOWS_PHONE
							Contract.Assume(observeScheduler != null);
#endif

							return CreateQbservable<TResult>(
								methodCallExpression.Arguments[0],
								_subscriberScheduler,
								observeScheduler);
						}
				}
			}

			return CreateQbservable<TResult>(expression, _subscriberScheduler, _observerScheduler);
		}

		protected abstract IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler);

#if !WINDOWS_PHONE
		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(AsyncRestClient != null);
			Contract.Invariant(SerializerFactory != null);
			Contract.Invariant(_subscriberScheduler != null);
			Contract.Invariant(_observerScheduler != null);
		}
#endif
	}

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

	internal class PollingRestQueryableProvider : RestQueryableProviderBase
	{
		private readonly TimeSpan _frequency;

		public PollingRestQueryableProvider(
			TimeSpan frequency,
			IAsyncRestClientFactory asyncRestClient,
			ISerializerFactory serializerFactory,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
			_frequency = frequency;
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new PollingRestObservable<TResult>(_frequency, AsyncRestClient, SerializerFactory, expression, subscriberScheduler, observerScheduler);
		}
	}
}