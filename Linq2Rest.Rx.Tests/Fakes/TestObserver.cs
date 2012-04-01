namespace Linq2Rest.Rx.Tests.Fakes
{
	using System;

	public class TestObserver : IObserver<string>
	{
		public void OnNext(string value)
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}
	}
}
