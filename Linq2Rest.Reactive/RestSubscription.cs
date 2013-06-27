namespace Linq2Rest.Reactive
{
	using System;
	using System.Diagnostics.Contracts;

	internal class RestSubscription<T> : IDisposable
	{
		private readonly IObserver<T> _observer;
		private readonly Action<IObserver<T>> _unsubscription;

		public RestSubscription(IObserver<T> observer, Action<IObserver<T>> unsubscription)
		{
			Contract.Requires(observer != null);
			Contract.Requires(unsubscription != null);

			_observer = observer;
			_unsubscription = unsubscription;
		}

		public void Dispose()
		{
			_unsubscription(_observer);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_observer != null);
			Contract.Invariant(_unsubscription != null);
		}
	}
}