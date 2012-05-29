// <copyright file="RestContextTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for RestContext`1</summary>
    [PexClass(typeof(RestContext<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestContextTTests
    {
        /// <summary>Test stub for .ctor(IRestClient, ISerializerFactory)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public RestContext<T> Constructor<T>(IRestClient client, ISerializerFactory serializerFactory)
        {
            RestContext<T> target = new RestContext<T>(client, serializerFactory);
            return target;
            // TODO: add assertions to method RestContextTTests.Constructor(IRestClient, ISerializerFactory)
        }

        /// <summary>Test stub for Dispose()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public void Dispose<T>([PexAssumeUnderTest]RestContext<T> target)
        {
            target.Dispose();
            // TODO: add assertions to method RestContextTTests.Dispose(RestContext`1<!!0>)
        }

        /// <summary>Test stub for get_Query()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQueryable<T> QueryGet<T>([PexAssumeUnderTest]RestContext<T> target)
        {
            IQueryable<T> result = target.Query;
            return result;
            // TODO: add assertions to method RestContextTTests.QueryGet(RestContext`1<!!0>)
        }
    }
}
