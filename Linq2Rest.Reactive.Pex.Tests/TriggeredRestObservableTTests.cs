// <copyright file="TriggeredRestObservableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for TriggeredRestObservable`1</summary>
    [PexClass(typeof(TriggeredRestObservable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class TriggeredRestObservableTTests
    {
        /// <summary>Test stub for .ctor(IObservable`1&lt;Unit&gt;, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal TriggeredRestObservable<T> Constructor<T>(
            IObservable<Unit> trigger,
            IAsyncRestClientFactory restClient,
            ISerializerFactory serializerFactory,
            Expression expression,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            TriggeredRestObservable<T> target
               = new TriggeredRestObservable<T>(trigger, restClient, serializerFactory, 
                                                expression, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method TriggeredRestObservableTTests.Constructor(IObservable`1<Unit>, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)
        }

        /// <summary>Test stub for get_Provider()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservableProvider ProviderGet<T>([PexAssumeUnderTest]TriggeredRestObservable<T> target)
        {
            IQbservableProvider result = target.Provider;
            return result;
            // TODO: add assertions to method TriggeredRestObservableTTests.ProviderGet(TriggeredRestObservable`1<!!0>)
        }

        /// <summary>Test stub for Subscribe(IObserver`1&lt;!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IDisposable Subscribe<T>(
            [PexAssumeUnderTest]TriggeredRestObservable<T> target,
            IObserver<T> observer
        )
        {
            IDisposable result = target.Subscribe(observer);
            return result;
            // TODO: add assertions to method TriggeredRestObservableTTests.Subscribe(TriggeredRestObservable`1<!!0>, IObserver`1<!!0>)
        }
    }
}
