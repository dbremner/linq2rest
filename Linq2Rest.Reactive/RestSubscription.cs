namespace Linq2Rest.Reactive
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif

	internal class RestSubscription<T> : IDisposable
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
}