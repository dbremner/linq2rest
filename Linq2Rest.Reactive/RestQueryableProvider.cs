// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestQueryableProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestQueryableProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Provider;

	internal class RestQueryableProvider : RestQueryableProviderBase
	{
		private readonly IMemberNameResolver _memberNameResolver;

		public RestQueryableProvider(IAsyncRestClientFactory asyncRestClient, ISerializerFactory serializerFactory, IMemberNameResolver memberNameResolver, IScheduler subscriberScheduler, IScheduler observerScheduler)
			: base(asyncRestClient, serializerFactory, subscriberScheduler, observerScheduler)
		{
			_memberNameResolver = memberNameResolver;
			Contract.Requires(asyncRestClient != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(subscriberScheduler != null);
			Contract.Requires(memberNameResolver != null);
			Contract.Requires(observerScheduler != null);
		}

		protected override IQbservable<TResult> CreateQbservable<TResult>(Expression expression, IScheduler subscriberScheduler, IScheduler observerScheduler)
		{
			return new InnerRestObservable<TResult>(AsyncRestClient, SerializerFactory, _memberNameResolver, expression, subscriberScheduler, observerScheduler);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_memberNameResolver != null);
		}
	}
}