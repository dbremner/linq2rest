namespace Linq2Rest.Reactive
{
	using System;
	using System.Reactive.Concurrency;
	using Linq2Rest.Provider;

	internal class ReactiveParameterBuilder : ParameterBuilder
	{
		public ReactiveParameterBuilder(Uri serviceBase)
			: base(serviceBase)
		{
		}

		public IScheduler SubscribeScheduler { get; set; }

		public IScheduler ObserveScheduler { get; set; }
	}
}
