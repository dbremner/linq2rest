// <copyright file="TriggeredRestQueryableProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for TriggeredRestQueryableProvider</summary>
    [PexClass(typeof(TriggeredRestQueryableProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class TriggeredRestQueryableProviderTests
    {
        /// <summary>Test stub for .ctor(IObservable`1&lt;Unit&gt;, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)</summary>
        [PexMethod]
        internal TriggeredRestQueryableProvider Constructor(
            IObservable<Unit> trigger,
            IAsyncRestClientFactory asyncRestClient,
            ISerializerFactory serializerFactory,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            TriggeredRestQueryableProvider target
               = new TriggeredRestQueryableProvider(trigger, asyncRestClient, 
                                                    serializerFactory, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method TriggeredRestQueryableProviderTests.Constructor(IObservable`1<Unit>, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)
        }
    }
}
