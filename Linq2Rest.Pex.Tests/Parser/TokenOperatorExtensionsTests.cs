// <copyright file="TokenOperatorExtensionsTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Text.RegularExpressions;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for TokenOperatorExtensions</summary>
    [PexClass(typeof(TokenOperatorExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class TokenOperatorExtensionsTests
    {
        /// <summary>Test stub for EnclosedMatch(String)</summary>
        [PexMethod]
        internal Match EnclosedMatch(string expression)
        {
            Match result = TokenOperatorExtensions.EnclosedMatch(expression);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.EnclosedMatch(String)
        }

        /// <summary>Test stub for IsArithmetic(String)</summary>
        [PexMethod]
        internal bool IsArithmetic(string operation)
        {
            bool result = TokenOperatorExtensions.IsArithmetic(operation);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsArithmetic(String)
        }

        /// <summary>Test stub for IsCombinationOperation(String)</summary>
        [PexMethod]
        internal bool IsCombinationOperation(string operation)
        {
            bool result = TokenOperatorExtensions.IsCombinationOperation(operation);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsCombinationOperation(String)
        }

        /// <summary>Test stub for IsEnclosed(String)</summary>
        [PexMethod]
        internal bool IsEnclosed(string expression)
        {
            bool result = TokenOperatorExtensions.IsEnclosed(expression);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsEnclosed(String)
        }

        /// <summary>Test stub for IsImpliedBoolean(String)</summary>
        [PexMethod]
        internal bool IsImpliedBoolean(string expression)
        {
            bool result = TokenOperatorExtensions.IsImpliedBoolean(expression);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsImpliedBoolean(String)
        }

        /// <summary>Test stub for IsOperation(String)</summary>
        [PexMethod]
        internal bool IsOperation(string operation)
        {
            bool result = TokenOperatorExtensions.IsOperation(operation);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsOperation(String)
        }

        /// <summary>Test stub for IsStringEnd(String)</summary>
        [PexMethod]
        internal bool IsStringEnd(string expression)
        {
            bool result = TokenOperatorExtensions.IsStringEnd(expression);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsStringEnd(String)
        }

        /// <summary>Test stub for IsStringStart(String)</summary>
        [PexMethod]
        internal bool IsStringStart(string expression)
        {
            bool result = TokenOperatorExtensions.IsStringStart(expression);
            return result;
            // TODO: add assertions to method TokenOperatorExtensionsTests.IsStringStart(String)
        }
    }
}
