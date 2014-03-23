// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InnerRestObservableBase.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
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
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Reflection;
	using System.Threading;
	using Linq2Rest.Provider;

	internal abstract class InnerRestObservableBase<T, TSource> : IQbservable<T>
	{
		protected readonly static MethodInfo CreateMethodInfo = typeof(ISerializerFactory).GetMethods().First(x => x.Name == "Create" && x.GetGenericArguments().Length == 1).GetGenericMethodDefinition();
		protected readonly static MethodInfo AliasCreateMethodInfo = typeof(ISerializerFactory).GetMethods().First(x => x.Name == "Create" && x.GetGenericArguments().Length == 2).GetGenericMethodDefinition();

		private readonly IAsyncRestClientFactory _restClient;
		private readonly ISerializerFactory _serializerFactory;
		private IDisposable _internalSubscription;
		private IDisposable _subscribeSubscription;

		internal InnerRestObservableBase(
			IAsyncRestClientFactory restClient,
			ISerializerFactory serializerFactory,
			IMemberNameResolver memberNameResolver,
			Expression expression,
			IScheduler subscriberScheduler,
			IScheduler observerScheduler)
		{
			Contract.Requires(restClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(memberNameResolver != null);

			Observers = new List<IObserver<T>>();
			Processor = new AsyncExpressionProcessor(new ExpressionWriter(memberNameResolver), memberNameResolver);
			_restClient = restClient;
			_serializerFactory = serializerFactory;
			SubscriberScheduler = subscriberScheduler ?? CurrentThreadScheduler.Instance;
			ObserverScheduler = observerScheduler ?? CurrentThreadScheduler.Instance;
			Expression = expression ?? Expression.Constant(this);
		}

		public ISerializerFactory SerializerFactory
		{
			get { return _serializerFactory; }
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
			if (_internalSubscription != null)
			{
				_internalSubscription.Dispose();
			}

			Observers.Add(observer);
			_subscribeSubscription = SubscriberScheduler
				.Schedule(
						  observer,
						  (s, o) =>
						  {
							  var filter = Expression as MethodCallExpression;
							  var parameterBuilder = new ParameterBuilder(RestClient.ServiceBase, typeof(TSource));
							  IObservable<T> source = null;
							  using (var waitHandle = new ManualResetEventSlim(false))
							  {
								  SubscriberScheduler.Schedule(() =>
								  {
									  try
									  {
										  source = Processor.ProcessMethodCall(
											  filter,
											  parameterBuilder,
											  GetResults,
											  GetIntermediateResults);
									  }
									  catch (Exception e)
									  {
										  source = Observable.Throw(e, default(T));
									  }
									  finally
									  {
										  waitHandle.Set();
									  }
								  });
								  waitHandle.Wait();
							  }

							  _internalSubscription = source
								  .Subscribe(new ObserverPublisher<T>(Observers, ObserverScheduler));
						  });
			return new RestSubscription<T>(observer, Unsubscribe);
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
				.Select(x => ReadIntermediateResponse(type, builder.SourceType, x));
		}

		protected IObservable<IEnumerable<T>> GetResults(ParameterBuilder builder)
		{
			Contract.Requires(builder != null);

			var fullUri = builder.GetFullUri();
			var client = RestClient.Create(fullUri);

			return Observable.FromAsync(client.Download)
				.Select(x => ReadResponse(x, builder.SourceType));
		}

		private IEnumerable ReadIntermediateResponse(Type type, Type aliasType, Stream response)
		{
			var genericMethod = ReflectionHelper.CreateMethod.MakeGenericMethod(type);
#if !SILVERLIGHT
			dynamic serializer = GetSerializer(type, aliasType);
			var resultSet = serializer.DeserializeList(response);
#else
			var serializer = genericMethod.Invoke(_serializerFactory, null);
			var deserializeListMethod = serializer.GetType().GetMethod("DeserializeList");
			var resultSet = deserializeListMethod.Invoke(serializer, new object[] { response });
#endif
			return resultSet as IEnumerable;
		}

		private ISerializer<T> GetSerializer(Type aliasType)
		{
			if (aliasType == null)
			{
				return _serializerFactory.Create<T>();
			}

			var method = AliasCreateMethodInfo.MakeGenericMethod(typeof(T), aliasType);

			return (ISerializer<T>)method.Invoke(_serializerFactory, null);
		}

		private object GetSerializer(Type itemType, Type aliasType)
		{
			if (aliasType == null)
			{
				var method = CreateMethodInfo.MakeGenericMethod(itemType);

				return method.Invoke(_serializerFactory, null);
			}

			var aliasMethod = AliasCreateMethodInfo.MakeGenericMethod(typeof(T), aliasType);

			return aliasMethod.Invoke(_serializerFactory, null);
		}

		private IEnumerable<T> ReadResponse(Stream stream, Type sourceType)
		{
			Contract.Requires(stream != null);

			dynamic serializer = GetSerializer(sourceType);

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

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_restClient != null);
			Contract.Invariant(_serializerFactory != null);
			Contract.Invariant(Observers != null);
		}
	}
}