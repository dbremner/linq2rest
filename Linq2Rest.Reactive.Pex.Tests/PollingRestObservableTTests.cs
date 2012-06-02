// <copyright file="PollingRestObservableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for PollingRestObservable`1</summary>
    [PexClass(typeof(PollingRestObservable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class PollingRestObservableTTests
    {
        /// <summary>Test stub for .ctor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal PollingRestObservable<T> Constructor<T>(
            TimeSpan frequency,
            IAsyncRestClientFactory restClient,
            ISerializerFactory serializerFactory,
            Expression expression,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            PollingRestObservable<T> target
               = new PollingRestObservable<T>(frequency, restClient, serializerFactory, 
                                              expression, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method PollingRestObservableTTests.Constructor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)
        }

        /// <summary>Test stub for get_Provider()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservableProvider ProviderGet<T>([PexAssumeUnderTest]PollingRestObservable<T> target)
        {
            IQbservableProvider result = target.Provider;
            return result;
            // TODO: add assertions to method PollingRestObservableTTests.ProviderGet(PollingRestObservable`1<!!0>)
        }
    }
}
