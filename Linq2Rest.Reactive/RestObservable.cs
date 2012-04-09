// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !SILVERLIGHT
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Reflection;
	using System.Threading.Tasks;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines an observable REST query.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object returned by the REST service.</typeparam>
	public class RestObservable<T> : IQbservable<T>
	{
		private readonly IAsyncRestClientFactory _restClient;
		private readonly ISerializerFactory _serializerFactory;
		private readonly IScheduler _subscriberScheduler;
		private readonly IScheduler _observerScheduler;
		private readonly IList<IObserver<T>> _observers = new List<IObserver<T>>();
		private readonly IAsyncExpressionProcessor _processor;
		private IDisposable _subscribeSubscription;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestObservable{T}"/> class.
		/// </summary>
		/// <param name="restClient">The <see cref="IAsyncRestClientFactory"/> to create a web client.</param>
		/// <param name="serializerFactory">The <see cref="ISerializerFactory"/> to create the serializer.</param>
		public RestObservable(IAsyncRestClientFactory restClient, ISerializerFactory serializerFactory)
			: this(restClient, serializerFactory, null, Scheduler.Immediate, Scheduler.Immediate)
		{
#if !SILVERLIGHT
			Contract.Requires<ArgumentNullException>(restClient != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
#endif
		}

		internal RestObservable(IAsyncRestClientFactory restClient, ISerializerFactory serializerFactory, Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
#if !SILVERLIGHT
			Contract.Requires(restClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(observerScheduler != null);
#endif

			_processor = new AsyncExpressionProcessor(new Provider.ExpressionVisitor()); // new ExpressionProcessor(new Provider.ExpressionVisitor());
			_restClient = restClient;
			_serializerFactory = serializerFactory;
			_subscriberScheduler = subscriberScheduler;
			_observerScheduler = observerScheduler;
			Expression = expression ?? Expression.Constant(this);
			Provider = new RestQueryableProvider(restClient, serializerFactory, _subscriberScheduler, _observerScheduler);
		}

		/// <summary>
		/// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQbservable is executed.
		/// </summary>
		public Type ElementType
		{
			get
			{
				return typeof(T);
			}
		}

		/// <summary>
		/// Gets the expression tree that is associated with the instance of IQbservable.
		/// </summary>
		public Expression Expression { get; private set; }

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		public IQbservableProvider Provider { get; private set; }

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
							  var parameterBuilder = new ParameterBuilder(_restClient.ServiceBase);

							  _processor.ProcessMethodCall(
														   filter,
														   parameterBuilder,
														   GetResults,
														   GetIntermediateResults)
								  .ContinueWith(OnGotResult, TaskContinuationOptions.PreferFairness);
						  });
			return new RestSubscription(observer, Unsubscribe);
		}

		private Task<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = _restClient.Create(builder.GetFullUri());

			return Task.Factory
				.FromAsync<string>(client.BeginGetResult, client.EndGetResult, null)
				.ContinueWith<IEnumerable>(x => ReadIntermediateResponse(type, x.Result));
		}

		private IEnumerable ReadIntermediateResponse(Type type, string response)
		{
			var genericMethod = ReflectionHelper.CreateMethod.MakeGenericMethod(type);
#if !SILVERLIGHT
			dynamic serializer = genericMethod.Invoke(_serializerFactory, null);
			var resultSet = serializer.DeserializeList(response);
#else
			var serializer = genericMethod.Invoke(_serializerFactory, null);
			var deserializeListMethod = serializer.GetType().GetMethod("DeserializeList", BindingFlags.Public);
			var resultSet = deserializeListMethod.Invoke(serializer, new object[] { response });
#endif
			return resultSet as IEnumerable;
		}

		private Task<IList<T>> GetResults(ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var client = _restClient.Create(fullUri);

			return Task.Factory
				.FromAsync<string>(client.BeginGetResult, client.EndGetResult, null)
				.ContinueWith<IList<T>>(ReadResponse);
		}

		private IList<T> ReadResponse(Task<string> downloadTask)
		{
			var serializer = _serializerFactory.Create<T>();

			return serializer.DeserializeList(downloadTask.Result);
		}

		private void Unsubscribe(IObserver<T> observer)
		{
			if (_observers.Contains(observer))
			{
				_observers.Remove(observer);
				if (_observers.Count == 0)
				{
					_subscribeSubscription.Dispose();
				}
			}
		}

		private void OnGotResult(Task<IObservable<T>> task)
		{
			if (task.IsFaulted)
			{
				throw task.Exception;
			}

			if (task.IsCanceled)
			{
				foreach (var observer in _observers)
				{
					var observer1 = observer;
					_observerScheduler.Schedule(observer1.OnCompleted);
				}

				return;
			}

			task.Result
				.Subscribe(
						   t =>
						   {
							   foreach (var observer in _observers)
							   {
								   var observer1 = observer;
								   _observerScheduler.Schedule(() => observer1.OnNext(t));
							   }
						   },
						   t =>
						   {
							   foreach (var observer in _observers)
							   {
								   var observer1 = observer;
								   _observerScheduler.Schedule(() => observer1.OnError(t));
							   }
						   },
							() =>
							{
								foreach (var observer in _observers)
								{
									var observer1 = observer;
									_observerScheduler.Schedule(observer1.OnCompleted);
								}
							});
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
	}
}
