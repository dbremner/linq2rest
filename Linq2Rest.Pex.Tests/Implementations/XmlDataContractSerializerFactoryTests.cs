// <copyright file="XmlDataContractSerializerFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for XmlDataContractSerializerFactory</summary>
    [PexClass(typeof(XmlDataContractSerializerFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class XmlDataContractSerializerFactoryTests
    {
        /// <summary>Test stub for .ctor(IEnumerable`1&lt;Type&gt;)</summary>
        [PexMethod]
        public XmlDataContractSerializerFactory Constructor(IEnumerable<Type> knownTypes)
        {
            XmlDataContractSerializerFactory target
               = new XmlDataContractSerializerFactory(knownTypes);
            return target;
            // TODO: add assertions to method XmlDataContractSerializerFactoryTests.Constructor(IEnumerable`1<Type>)
        }

        /// <summary>Test stub for Create()</summary>
        [PexGenericArguments(typeof(int))]
		[PexMethod(MaxConditions = 1000)]
        public ISerializer<T> Create<T>([PexAssumeUnderTest]XmlDataContractSerializerFactory target)
        {
            ISerializer<T> result = target.Create<T>();
            return result;
            // TODO: add assertions to method XmlDataContractSerializerFactoryTests.Create(XmlDataContractSerializerFactory)
        }
    }
}
