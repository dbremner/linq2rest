// <copyright file="RestObservableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive;
using System.Reactive.Linq;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for RestObservable`1</summary>
    [PexClass(typeof(RestObservable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestObservableTTests
    {
        /// <summary>Test stub for .ctor(IAsyncRestClientFactory, ISerializerFactory)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public RestObservable<T> Constructor<T>(
            IAsyncRestClientFactory restClientFactory,
            ISerializerFactory serializerFactory
        )
        {
            RestObservable<T> target
               = new RestObservable<T>(restClientFactory, serializerFactory);
            return target;
            // TODO: add assertions to method RestObservableTTests.Constructor(IAsyncRestClientFactory, ISerializerFactory)
        }

        /// <summary>Test stub for Create()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQbservable<T> Create<T>([PexAssumeUnderTest]RestObservable<T> target)
        {
            IQbservable<T> result = target.Create();
            return result;
            // TODO: add assertions to method RestObservableTTests.Create(RestObservable`1<!!0>)
        }

        /// <summary>Test stub for Poll(TimeSpan)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQbservable<T> Poll<T>(
            [PexAssumeUnderTest]RestObservable<T> target,
            TimeSpan interval
        )
        {
            IQbservable<T> result = target.Poll(interval);
            return result;
            // TODO: add assertions to method RestObservableTTests.Poll(RestObservable`1<!!0>, TimeSpan)
        }

        /// <summary>Test stub for Requery(TimeSpan)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQbservable<T> Requery<T>(
            [PexAssumeUnderTest]RestObservable<T> target,
            TimeSpan interval
        )
        {
            IQbservable<T> result = target.Requery(interval);
            return result;
            // TODO: add assertions to method RestObservableTTests.Requery(RestObservable`1<!!0>, TimeSpan)
        }

        /// <summary>Test stub for Triggered(IObservable`1&lt;Unit&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQbservable<T> Triggered<T>(
            [PexAssumeUnderTest]RestObservable<T> target,
            IObservable<Unit> trigger
        )
        {
            IQbservable<T> result = target.Triggered(trigger);
            return result;
            // TODO: add assertions to method RestObservableTTests.Triggered(RestObservable`1<!!0>, IObservable`1<Unit>)
        }
    }
}
