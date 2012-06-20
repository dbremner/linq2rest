// <copyright file="PollingRestQueryableProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive.Concurrency;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for PollingRestQueryableProvider</summary>
    [PexClass(typeof(PollingRestQueryableProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class PollingRestQueryableProviderTests
    {
        /// <summary>Test stub for .ctor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)</summary>
        [PexMethod]
        internal PollingRestQueryableProvider Constructor(
            TimeSpan frequency,
            IAsyncRestClientFactory asyncRestClient,
            ISerializerFactory serializerFactory,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            PollingRestQueryableProvider target
               = new PollingRestQueryableProvider(frequency, asyncRestClient, 
                                                  serializerFactory, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method PollingRestQueryableProviderTests.Constructor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)
        }
    }
}
