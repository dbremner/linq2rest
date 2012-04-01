namespace Linq2Rest.Rx
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Linq2Rest.Provider;

	public class RestObservable<T> : IQbservable<T>
	{
		private readonly IAsyncRestClientFactory _restClient;
		private readonly ISerializerFactory _serializerFactory;
		private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
		private readonly IAsyncExpressionProcessor _processor;

		public RestObservable(IAsyncRestClientFactory restClient, ISerializerFactory serializerFactory)
		{
			_processor = new AsyncExpressionProcessor(new Provider.ExpressionVisitor()); // new ExpressionProcessor(new Provider.ExpressionVisitor());
			_restClient = restClient;
			_serializerFactory = serializerFactory;
			Expression = Expression.Constant(this);
			Provider = new RestQueryableProvider(restClient, serializerFactory);
		}

		internal RestObservable(IAsyncRestClientFactory restClient, ISerializerFactory serializerFactory, Expression expression)
			: this(restClient, serializerFactory)
		{
			Expression = expression;
		}

		public Type ElementType
		{
			get
			{
				return typeof(T);
			}
		}

		public Expression Expression { get; private set; }

		public IQbservableProvider Provider { get; private set; }

		public IDisposable Subscribe(IObserver<T> observer)
		{
			_observers.Add(observer);
			var filter = (MethodCallExpression)Expression;
			var parameterBuilder = new ParameterBuilder(_restClient.ServiceBase);

			_processor.ProcessMethodCall(filter, parameterBuilder, GetResults, GetIntermediateResults)
				.ContinueWith(OnGotResult, TaskContinuationOptions.PreferFairness);

			return new RestSubscription(observer, Unsubscribe);
		}

		private Task<IEnumerable> GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var client = _restClient.Create(builder.GetFullUri());

			return Task.Factory
				.FromAsync<string>(client.BeginGetResult, client.EndGetResult, null)
				.ContinueWith<IEnumerable>(ReadIntermediateResponse);
		}

		private IEnumerable ReadIntermediateResponse(Task<string> downloadTask)
		{
			return new List<T>();
		}

		private Task<IList<T>> GetResults(ParameterBuilder builder)
		{
			var client = _restClient.Create(builder.GetFullUri());

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
			}
		}

		private void OnGotResult(Task<IList<T>> task)
		{
			foreach (var observer in _observers)
			{
				foreach (var result in task.Result)
				{
					observer.OnNext(result);
				}
			}

			foreach (var observer in _observers)
			{
				observer.OnCompleted();
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
	}
}
