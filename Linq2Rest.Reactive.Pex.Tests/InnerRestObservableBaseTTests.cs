// <copyright file="InnerRestObservableBaseTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reactive.Linq;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for InnerRestObservableBase`1</summary>
    [PexClass(typeof(InnerRestObservableBase<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class InnerRestObservableBaseTTests
    {
        /// <summary>Test stub for get_ElementType()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal Type ElementTypeGet<T>([PexAssumeNotNull]InnerRestObservableBase<T> target)
        {
            Type result = target.ElementType;
            return result;
            // TODO: add assertions to method InnerRestObservableBaseTTests.ElementTypeGet(InnerRestObservableBase`1<!!0>)
        }

        /// <summary>Test stub for get_Provider()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservableProvider ProviderGet<T>([PexAssumeNotNull]InnerRestObservableBase<T> target)
        {
            IQbservableProvider result = target.Provider;
            return result;
            // TODO: add assertions to method InnerRestObservableBaseTTests.ProviderGet(InnerRestObservableBase`1<!!0>)
        }

        /// <summary>Test stub for Subscribe(IObserver`1&lt;!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IDisposable Subscribe<T>(
            [PexAssumeNotNull]InnerRestObservableBase<T> target,
            IObserver<T> observer
        )
        {
            IDisposable result = target.Subscribe(observer);
            return result;
            // TODO: add assertions to method InnerRestObservableBaseTTests.Subscribe(InnerRestObservableBase`1<!!0>, IObserver`1<!!0>)
        }
    }
}
