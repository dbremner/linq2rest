// <copyright file="RestQueryableTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for RestQueryable`1</summary>
    [PexClass(typeof(RestQueryable<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestQueryableTTests
    {
        /// <summary>Test stub for .ctor(IRestClient, ISerializerFactory)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal RestQueryable<T> Constructor<T>(IRestClient client, ISerializerFactory serializerFactory)
        {
            RestQueryable<T> target = new RestQueryable<T>(client, serializerFactory);
            return target;
            // TODO: add assertions to method RestQueryableTTests.Constructor(IRestClient, ISerializerFactory)
        }

        /// <summary>Test stub for .ctor(IRestClient, ISerializerFactory, Expression)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal RestQueryable<T> Constructor01<T>(
            IRestClient client,
            ISerializerFactory serializerFactory,
            Expression expression
        )
        {
            RestQueryable<T> target
               = new RestQueryable<T>(client, serializerFactory, expression);
            return target;
            // TODO: add assertions to method RestQueryableTTests.Constructor01(IRestClient, ISerializerFactory, Expression)
        }

        /// <summary>Test stub for Dispose()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal void Dispose<T>([PexAssumeUnderTest]RestQueryable<T> target)
        {
            target.Dispose();
            // TODO: add assertions to method RestQueryableTTests.Dispose(RestQueryable`1<!!0>)
        }

        /// <summary>Test stub for get_ElementType()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal Type ElementTypeGet<T>([PexAssumeUnderTest]RestQueryable<T> target)
        {
            Type result = target.ElementType;
            return result;
            // TODO: add assertions to method RestQueryableTTests.ElementTypeGet(RestQueryable`1<!!0>)
        }

        /// <summary>Test stub for GetEnumerator()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IEnumerator<T> GetEnumerator<T>([PexAssumeUnderTest]RestQueryable<T> target)
        {
            IEnumerator<T> result = target.GetEnumerator();
            return result;
            // TODO: add assertions to method RestQueryableTTests.GetEnumerator(RestQueryable`1<!!0>)
        }
    }
}
