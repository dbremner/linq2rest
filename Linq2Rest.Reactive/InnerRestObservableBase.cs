// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InnerRestObservableBase.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the InnerRestObservableBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
	using Linq2Rest.Provider;

	internal abstract class InnerRestObservableBase<T> : IQbservable<T>
	{
		private readonly IAsyncRestClientFactory _restClient;
		private readonly ISerializerFactory _serializerFactory;
		private IDisposable _subscribeSubscription;
		private IDisposable _internalSubscription;

		internal InnerRestObservableBase(
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
		{
#if !WINDOWS_PHONE
			Contract.Requires(restClient != null);
			Contract.Requires(serializerFactory != null);
#endif

			Observers = new List<IObserver<T>>();
			Processor = new AsyncExpressionProcessor(new ExpressionWriter());
			_restClient = restClient;
			_serializerFactory = serializerFactory;
			SubscriberScheduler = subscriberScheduler ?? CurrentThreadScheduler.Instance;
			ObserverScheduler = observerScheduler ?? CurrentThreadScheduler.Instance;
			Expression = expression ?? Expression.Constant(this);
		}

		/// <summary>
		/// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQbservable is executed.
		/// </summary>
		public Type ElementType
		{
			get { return typeof(T); }
		}

		/// <summary>
		/// Gets the expression tree that is associated with the instance of IQbservable.
		/// </summary>
		public Expression Expression { get; private set; }

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public abstract IQbservableProvider Provider { get; }

		public ISerializerFactory SerializerFactory
		{
			get { return _serializerFactory; }
		}

		protected IAsyncRestClientFactory RestClient
		{
			get { return _restClient; }
		}

		protected IAsyncExpressionProcessor Processor { get; private set; }

		protected IList<IObserver<T>> Observers { get; private set; }

		protected IScheduler ObserverScheduler { get; private set; }

		protected IScheduler SubscriberScheduler { get; private set; }

		/// <summary>
		/// Notifies the provider that an observer is to receive notifications.
		/// </summary>
		/// <returns>
		/// A reference to an interface that allows observers to stop receiving notifications before the provider has finished sending them.
		/// </returns>
		/// <param name="observer">The object that is to receive notifications.</param>
		public virtual IDisposable Subscribe(IObserver<T> observer)
		{
			Observers.Add(observer);
			_subscribeSubscription = SubscriberScheduler
				.Schedule(
						  observer,
						  (s, o) =>
						  {
							  var filter = Expression as MethodCallExpression;
							  var parameterBuilder = new ParameterBuilder(RestClient.ServiceBase);

							  _internalSubscription = Processor.ProcessMethodCall(
																				   filter,
																				   parameterBuilder,
																				   GetResults,
																				   GetIntermediateResults)
								  .Subscribe(new ObserverPublisher(Observers, ObserverScheduler));
						  });
			return new RestSubscription(observer, Unsubscribe);
		}

		internal void ChangeMethod(HttpMethod method)
		{
			RestClient.SetMethod(method);
		}

		internal void SetInput(Stream stream)
		{
			Contract.Requires(stream != null);

			RestClient.SetInput(stream);
		}

		protected IObservable<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = RestClient.Create(builder.GetFullUri());

			return Observable.FromAsync(client.Download)
				.Select(x => ReadIntermediateResponse(type, x));
		}

		protected IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
#if !WINDOWS_PHONE
			Contract.Requires(builder != null);
#endif
			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);

			return Observable.FromAsync(client.Download)
				.Select(ReadResponse);
		}

		protected IEnumerable ReadIntermediateResponse(Type type, Stream response)
		{
			var genericMethod = ReflectionHelper.CreateMethod.MakeGenericMethod(type);
#if !SILVERLIGHT
			dynamic serializer = genericMethod.Invoke(SerializerFactory, null);
			var resultSet = serializer.DeserializeList(response);
#else
			var serializer = genericMethod.Invoke(_serializerFactory, null);
			var deserializeListMethod = serializer.GetType().GetMethod("DeserializeList");
			var resultSet = deserializeListMethod.Invoke(serializer, new object[] { response });
#endif
			return resultSet as IEnumerable;
		}

		private IEnumerable<T> ReadResponse(Stream stream)
		{
			var serializer = SerializerFactory.Create<T>();

			return serializer.DeserializeList(stream);
		}

		private void Unsubscribe(IObserver<T> observer)
		{
			if (!Observers.Contains(observer))
			{
				return;
			}

			Observers.Remove(observer);
			observer.OnCompleted();
			if (Observers.Count == 0)
			{
				if (_internalSubscription != null)
				{
					_internalSubscription.Dispose();
				}

				if (_subscribeSubscription != null)
				{
					_subscribeSubscription.Dispose();
				}
			}
		}

#if !WINDOWS_PHONE
		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_restClient != null);
			Contract.Invariant(_serializerFactory != null);
			Contract.Invariant(Observers != null);
		}
#endif

		internal class RestSubscription : IDisposable
		{
			private readonly IObserver<T> _observer;
			private readonly Action<IObserver<T>> _unsubscription;

			public RestSubscription(IObserver<T> observer, Action<IObserver<T>> unsubscription)
			{
#if !WINDOWS_PHONE
				Contract.Requires(observer != null);
				Contract.Requires(unsubscription != null);
#endif

				_observer = observer;
				_unsubscription = unsubscription;
			}

			public void Dispose()
			{
				_unsubscription(_observer);
			}

#if !WINDOWS_PHONE
			[ContractInvariantMethod]
			private void Invariants()
			{
				Contract.Invariant(_observer != null);
				Contract.Invariant(_unsubscription != null);
			}
#endif
		}

		internal class ObserverPublisher : IObserver<T>
		{
			private readonly IEnumerable<IObserver<T>> _observers;
			private readonly IScheduler _observerScheduler;

			public ObserverPublisher(IEnumerable<IObserver<T>> observers, IScheduler observerScheduler)
			{
#if !WINDOWS_PHONE
				Contract.Requires(observers != null);
				Contract.Requires(observerScheduler != null);
#endif
				_observers = observers;
				_observerScheduler = observerScheduler;
			}

			public void OnNext(T value)
			{
				foreach (var observer in _observers)
				{
					var observer1 = observer;
					_observerScheduler.Schedule(() => observer1.OnNext(value));
				}
			}

			public void OnError(Exception error)
			{
				foreach (var observer in _observers)
				{
					var observer1 = observer;
					_observerScheduler.Schedule(() => observer1.OnError(error));
				}
			}

			public void OnCompleted()
			{
				foreach (var observer in _observers)
				{
					var observer1 = observer;
					_observerScheduler.Schedule(observer1.OnCompleted);
				}
			}

#if !WINDOWS_PHONE
			[ContractInvariantMethod]
			private void Invariants()
			{
				Contract.Invariant(_observers != null);
				Contract.Invariant(_observerScheduler != null);
			}
#endif
		}
	}
}