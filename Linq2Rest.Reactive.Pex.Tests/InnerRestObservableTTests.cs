// <copyright file="InnerRestObservableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
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
    /// <summary>This class contains parameterized unit tests for InnerRestObservable`1</summary>
    [PexClass(typeof(InnerRestObservable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class InnerRestObservableTTests
    {
        /// <summary>Test stub for .ctor(IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal InnerRestObservable<T> Constructor<T>(
            IAsyncRestClientFactory restClient,
            ISerializerFactory serializerFactory,
            Expression expression,
            IScheduler subscriberScheduler,
            IScheduler observerScheduler
        )
        {
            InnerRestObservable<T> target
               = new InnerRestObservable<T>(restClient, serializerFactory, 
                                            expression, subscriberScheduler, observerScheduler);
            return target;
            // TODO: add assertions to method InnerRestObservableTTests.Constructor(IAsyncRestClientFactory, ISerializerFactory, Expression, IScheduler, IScheduler)
        }

        /// <summary>Test stub for get_Provider()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservableProvider ProviderGet<T>([PexAssumeUnderTest]InnerRestObservable<T> target)
        {
            IQbservableProvider result = target.Provider;
            return result;
            // TODO: add assertions to method InnerRestObservableTTests.ProviderGet(InnerRestObservable`1<!!0>)
        }
    }
}
