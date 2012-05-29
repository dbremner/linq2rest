// <copyright file="ParameterBuilderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for ParameterBuilder</summary>
    [PexClass(typeof(ParameterBuilder))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ParameterBuilderTests
    {
        /// <summary>Test stub for .ctor(Uri)</summary>
        [PexMethod]
        internal ParameterBuilder Constructor(Uri serviceBase)
        {
            ParameterBuilder target = new ParameterBuilder(serviceBase);
            return target;
            // TODO: add assertions to method ParameterBuilderTests.Constructor(Uri)
        }

        /// <summary>Test stub for GetFullUri()</summary>
        [PexMethod]
        internal Uri GetFullUri([PexAssumeUnderTest]ParameterBuilder target)
        {
            Uri result = target.GetFullUri();
            return result;
            // TODO: add assertions to method ParameterBuilderTests.GetFullUri(ParameterBuilder)
        }
    }
}
