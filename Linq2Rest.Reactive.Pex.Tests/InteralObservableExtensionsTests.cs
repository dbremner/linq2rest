// <copyright file="InteralObservableExtensionsTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Collections;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for InteralObservableExtensions</summary>
    [PexClass(typeof(InteralObservableExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class InteralObservableExtensionsTests
    {
        /// <summary>Test stub for ToQbservable(IEnumerable, Type)</summary>
        [PexMethod]
        internal object ToQbservable(IEnumerable enumerable, Type type)
        {
            object result = InteralObservableExtensions.ToQbservable(enumerable, type);
            return result;
            // TODO: add assertions to method InteralObservableExtensionsTests.ToQbservable(IEnumerable, Type)
        }
    }
}
