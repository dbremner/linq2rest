// <copyright file="RequeryingRestObservableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
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
    /// <summary>This class contains parameterized unit tests for RequeryingRestObservable`1</summary>
    [PexClass(typeof(RequeryingRestObservable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RequeryingRestObservableTTests
    {
        /// <summary>Test stub for .ctor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal RequeryingRestObservable<T> Constructor<T>(
            TimeSpan frequency,
            IAsyncRestClientFactory restClient,
            ISerializerFactory serializerFactory,
            Expression expression,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler)
        {
            RequeryingRestObservable<T> target
               = new RequeryingRestObservable<T>(frequency, restClient, serializerFactory, 
                                                 expression, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method RequeryingRestObservableTTests.Constructor(TimeSpan, IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)
        }

        /// <summary>Test stub for get_Provider()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservableProvider ProviderGet<T>([PexAssumeUnderTest]RequeryingRestObservable<T> target)
        {
            IQbservableProvider result = target.Provider;
            return result;
            // TODO: add assertions to method RequeryingRestObservableTTests.ProviderGet(RequeryingRestObservable`1<!!0>)
        }

        /// <summary>Test stub for Subscribe(IObserver`1&lt;!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IDisposable Subscribe<T>(
            [PexAssumeUnderTest]RequeryingRestObservable<T> target,
            IObserver<T> observer
        )
        {
            IDisposable result = target.Subscribe(observer);
            return result;
            // TODO: add assertions to method RequeryingRestObservableTTests.Subscribe(RequeryingRestObservable`1<!!0>, IObserver`1<!!0>)
        }
    }
}
