// <copyright file="FunctionTokenSetTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for FunctionTokenSet</summary>
    [PexClass(typeof(FunctionTokenSet))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class FunctionTokenSetTests
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        internal FunctionTokenSet Constructor()
        {
            FunctionTokenSet target = new FunctionTokenSet();
            return target;
            // TODO: add assertions to method FunctionTokenSetTests.Constructor()
        }

        /// <summary>Test stub for ToString()</summary>
        [PexMethod]
        internal string ToString01([PexAssumeUnderTest]FunctionTokenSet target)
        {
            string result = target.ToString();
            return result;
            // TODO: add assertions to method FunctionTokenSetTests.ToString01(FunctionTokenSet)
        }
    }
}
