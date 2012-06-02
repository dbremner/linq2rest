// <copyright file="RequeryingRestQueryableProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive.Concurrency;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for RequeryingRestQueryableProvider</summary>
    [PexClass(typeof(RequeryingRestQueryableProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RequeryingRestQueryableProviderTests
    {
        /// <summary>Test stub for .ctor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)</summary>
        [PexMethod]
        internal RequeryingRestQueryableProvider Constructor(
            TimeSpan frequency,
            IAsyncRestClientFactory asyncRestClient,
            ISerializerFactory serializerFactory,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            RequeryingRestQueryableProvider target
               = new RequeryingRestQueryableProvider(frequency, asyncRestClient, 
                                                     serializerFactory, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method RequeryingRestQueryableProviderTests.Constructor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)
        }
    }
}
