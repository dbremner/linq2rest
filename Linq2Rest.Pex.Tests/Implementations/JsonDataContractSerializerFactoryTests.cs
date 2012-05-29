// <copyright file="JsonDataContractSerializerFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for JsonDataContractSerializerFactory</summary>
    [PexClass(typeof(JsonDataContractSerializerFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class JsonDataContractSerializerFactoryTests
    {
        /// <summary>Test stub for .ctor(IEnumerable`1&lt;Type&gt;)</summary>
        [PexMethod]
        public JsonDataContractSerializerFactory Constructor(IEnumerable<Type> knownTypes)
        {
            JsonDataContractSerializerFactory target
               = new JsonDataContractSerializerFactory(knownTypes);
            return target;
            // TODO: add assertions to method JsonDataContractSerializerFactoryTests.Constructor(IEnumerable`1<Type>)
        }

        /// <summary>Test stub for Create()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public ISerializer<T> Create<T>([PexAssumeUnderTest]JsonDataContractSerializerFactory target)
        {
            ISerializer<T> result = target.Create<T>();
            return result;
            // TODO: add assertions to method JsonDataContractSerializerFactoryTests.Create(JsonDataContractSerializerFactory)
        }
    }
}
