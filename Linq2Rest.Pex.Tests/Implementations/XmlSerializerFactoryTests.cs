// <copyright file="XmlSerializerFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for XmlSerializerFactory</summary>
    [PexClass(typeof(XmlSerializerFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class XmlSerializerFactoryTests
    {
        /// <summary>Test stub for .ctor(IEnumerable`1&lt;Type&gt;)</summary>
        [PexMethod]
        public XmlSerializerFactory Constructor(IEnumerable<Type> knownTypes)
        {
            XmlSerializerFactory target = new XmlSerializerFactory(knownTypes);
            return target;
            // TODO: add assertions to method XmlSerializerFactoryTests.Constructor(IEnumerable`1<Type>)
        }

        /// <summary>Test stub for Create()</summary>
        [PexGenericArguments(typeof(int))]
		[PexMethod, PexAllowedException(typeof(ArgumentNullException))]
        public ISerializer<T> Create<T>([PexAssumeUnderTest]XmlSerializerFactory target)
        {
            ISerializer<T> result = target.Create<T>();
            return result;
            // TODO: add assertions to method XmlSerializerFactoryTests.Create(XmlSerializerFactory)
        }
    }
}
