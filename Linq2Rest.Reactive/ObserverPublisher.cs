namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Reactive.Concurrency;

	internal class ObserverPublisher<T> : IObserver<T>
	{
		private readonly IScheduler _observerScheduler;
		private readonly IEnumerable<IObserver<T>> _observers;

		public ObserverPublisher(IEnumerable<IObserver<T>> observers, IScheduler observerScheduler)
		{
			Contract.Requires(observers != null);
			Contract.Requires(observerScheduler != null);
		
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
		
		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_observers != null);
			Contract.Invariant(_observerScheduler != null);
		}
	}
}