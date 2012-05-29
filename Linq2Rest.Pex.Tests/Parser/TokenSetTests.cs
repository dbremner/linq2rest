// <copyright file="TokenSetTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for TokenSet</summary>
    [PexClass(typeof(TokenSet))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class TokenSetTests
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        internal TokenSet Constructor()
        {
            TokenSet target = new TokenSet();
            return target;
            // TODO: add assertions to method TokenSetTests.Constructor()
        }

        /// <summary>Test stub for get_Left()</summary>
        [PexMethod]
        internal string LeftGet([PexAssumeUnderTest]TokenSet target)
        {
            string result = target.Left;
            return result;
            // TODO: add assertions to method TokenSetTests.LeftGet(TokenSet)
        }

        /// <summary>Test stub for set_Left(String)</summary>
        [PexMethod]
        internal void LeftSet([PexAssumeUnderTest]TokenSet target, string value)
        {
            target.Left = value;
            // TODO: add assertions to method TokenSetTests.LeftSet(TokenSet, String)
        }

        /// <summary>Test stub for get_Operation()</summary>
        [PexMethod]
        internal string OperationGet([PexAssumeUnderTest]TokenSet target)
        {
            string result = target.Operation;
            return result;
            // TODO: add assertions to method TokenSetTests.OperationGet(TokenSet)
        }

        /// <summary>Test stub for set_Operation(String)</summary>
        [PexMethod]
        internal void OperationSet([PexAssumeUnderTest]TokenSet target, string value)
        {
            target.Operation = value;
            // TODO: add assertions to method TokenSetTests.OperationSet(TokenSet, String)
        }

        /// <summary>Test stub for get_Right()</summary>
        [PexMethod]
        internal string RightGet([PexAssumeUnderTest]TokenSet target)
        {
            string result = target.Right;
            return result;
            // TODO: add assertions to method TokenSetTests.RightGet(TokenSet)
        }

        /// <summary>Test stub for set_Right(String)</summary>
        [PexMethod]
        internal void RightSet([PexAssumeUnderTest]TokenSet target, string value)
        {
            target.Right = value;
            // TODO: add assertions to method TokenSetTests.RightSet(TokenSet, String)
        }

        /// <summary>Test stub for ToString()</summary>
        [PexMethod]
        internal string ToString01([PexAssumeUnderTest]TokenSet target)
        {
            string result = target.ToString();
            return result;
            // TODO: add assertions to method TokenSetTests.ToString01(TokenSet)
        }
    }
}
