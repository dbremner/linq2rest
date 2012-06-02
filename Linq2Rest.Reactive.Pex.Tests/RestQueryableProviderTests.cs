// <copyright file="RestQueryableProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive.Concurrency;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for RestQueryableProvider</summary>
    [PexClass(typeof(RestQueryableProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestQueryableProviderTests
    {
        /// <summary>Test stub for .ctor(IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)</summary>
        [PexMethod]
        internal RestQueryableProvider Constructor(
            IAsyncRestClientFactory asyncRestClient,
            ISerializerFactory serializerFactory,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            RestQueryableProvider target
               = new RestQueryableProvider(asyncRestClient, serializerFactory, 
                                           subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method RestQueryableProviderTests.Constructor(IAsyncRestClientFactory, ISerializerFactory, IScheduler, IScheduler)
        }
    }
}
