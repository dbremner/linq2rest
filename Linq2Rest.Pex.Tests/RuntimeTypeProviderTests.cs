// <copyright file="RuntimeTypeProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using System.Reflection;
using Linq2Rest;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest
{
    /// <summary>This class contains parameterized unit tests for RuntimeTypeProvider</summary>
    [PexClass(typeof(RuntimeTypeProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RuntimeTypeProviderTests
    {
        /// <summary>Test stub for .ctor(IMemberNameResolver)</summary>
        [PexMethod]
        public RuntimeTypeProvider Constructor(IMemberNameResolver nameResolver)
        {
            RuntimeTypeProvider target = new RuntimeTypeProvider(nameResolver);
            return target;
            // TODO: add assertions to method RuntimeTypeProviderTests.Constructor(IMemberNameResolver)
        }

        /// <summary>Test stub for Get(Type, IEnumerable`1&lt;MemberInfo&gt;)</summary>
        [PexMethod]
        public Type Get(
            [PexAssumeUnderTest]RuntimeTypeProvider target,
            Type sourceType,
            IEnumerable<MemberInfo> properties
        )
        {
            Type result = target.Get(sourceType, properties);
            return result;
            // TODO: add assertions to method RuntimeTypeProviderTests.Get(RuntimeTypeProvider, Type, IEnumerable`1<MemberInfo>)
        }
    }
}
