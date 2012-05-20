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
	using Linq2Rest.Provider;

	internal abstract class InnerRestObservableBase<T> : IQbservable<T>
	{
		private readonly IAsyncRestClientFactory _restClient;
		private readonly ISerializerFactory _serializerFactory;
		private readonly IScheduler _subscriberScheduler;
		private readonly IScheduler _observerScheduler;
		private readonly IList<IObserver<T>> _observers = new List<IObserver<T>>();
		private readonly IAsyncExpressionProcessor _processor;
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

			_processor = new AsyncExpressionProcessor(new Provider.ExpressionVisitor());
			_restClient = restClient;
			_serializerFactory = serializerFactory;
			_subscriberScheduler = subscriberScheduler ?? Scheduler.CurrentThread;
			_observerScheduler = observerScheduler ?? Scheduler.CurrentThread;
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

		protected IAsyncRestClientFactory RestClient
		{
			get { return _restClient; }
		}

		protected ISerializerFactory SerializerFactory
		{
			get { return _serializerFactory; }
		}

		/// <summary>
		/// Notifies the provider that an observer is to receive notifications.
		/// </summary>
		/// <returns>
		/// A reference to an interface that allows observers to stop receiving notifications before the provider has finished sending them.
		/// </returns>
		/// <param name="observer">The object that is to receive notifications.</param>
		public IDisposable Subscribe(IObserver<T> observer)
		{
			_observers.Add(observer);
			_subscribeSubscription = _subscriberScheduler
				.Schedule(
						  observer,
						  (s, o) =>
						  {
							  var filter = Expression as MethodCallExpression;
							  var parameterBuilder = new ParameterBuilder(RestClient.ServiceBase);

							  _internalSubscription = _processor.ProcessMethodCall(
																				   filter,
																				   parameterBuilder,
																				   GetResults,
																				   GetIntermediateResults)
								  .Subscribe(new ObserverPublisher(_observers, _observerScheduler));
						  });
			return new RestSubscription(observer, Unsubscribe);
		}

		protected virtual IObservable<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = RestClient.Create(builder.GetFullUri());

			return Observable
				.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)
				.Invoke()
				.Select(x => ReadIntermediateResponse(type, x));
		}

		protected virtual IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);

			return Observable
				.FromAsyncPattern<Stream>(client.BeginGetResult, client.EndGetResult)
				.Invoke()
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
			if (!_observers.Contains(observer))
			{
				return;
			}

			_observers.Remove(observer);
			observer.OnCompleted();
			if (_observers.Count == 0)
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

		private class RestSubscription : IDisposable
		{
			private readonly IObserver<T> _observer;
			private readonly Action<IObserver<T>> _unsubscription;

			public RestSubscription(IObserver<T> observer, Action<IObserver<T>> unsubscription)
			{
				_observer = observer;
				_unsubscription = unsubscription;
			}

			public void Dispose()
			{
				_unsubscription(_observer);
			}
		}

		private class ObserverPublisher : IObserver<T>
		{
			private readonly IEnumerable<IObserver<T>> _observers;
			private readonly IScheduler _observerScheduler;

			public ObserverPublisher(IEnumerable<IObserver<T>> observers, IScheduler observerScheduler)
			{
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
		}
	}
}