// <copyright file="ModelFilterExtensionsTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Linq2Rest;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest
{
    /// <summary>This class contains parameterized unit tests for ModelFilterExtensions</summary>
    [PexClass(typeof(ModelFilterExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ModelFilterExtensionsTests
    {
        /// <summary>Test stub for Filter(IEnumerable`1&lt;!!0&gt;, NameValueCollection)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IEnumerable<object> Filter<T>(IEnumerable<T> source, NameValueCollection query)
        {
            IEnumerable<object> result = ModelFilterExtensions.Filter<T>(source, query);
            return result;
            // TODO: add assertions to method ModelFilterExtensionsTests.Filter(IEnumerable`1<!!0>, NameValueCollection)
        }

        /// <summary>Test stub for Filter(IEnumerable`1&lt;!!0&gt;, IModelFilter`1&lt;!!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IEnumerable<object> Filter01<T>(IEnumerable<T> source, IModelFilter<T> filter)
        {
            IEnumerable<object> result = ModelFilterExtensions.Filter<T>(source, filter);
            return result;
            // TODO: add assertions to method ModelFilterExtensionsTests.Filter01(IEnumerable`1<!!0>, IModelFilter`1<!!0>)
        }
    }
}
