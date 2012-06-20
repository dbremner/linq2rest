// <copyright file="ExpressionTokenizerTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for ExpressionTokenizer</summary>
    [PexClass(typeof(ExpressionTokenizer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ExpressionTokenizerTests
    {
        /// <summary>Test stub for GetAnyAllFunctionTokens(String)</summary>
        [PexMethod]
        internal TokenSet GetAnyAllFunctionTokens(string filter)
        {
            TokenSet result = ExpressionTokenizer.GetAnyAllFunctionTokens(filter);
            return result;
            // TODO: add assertions to method ExpressionTokenizerTests.GetAnyAllFunctionTokens(String)
        }

        /// <summary>Test stub for GetArithmeticToken(String)</summary>
        [PexMethod]
        internal TokenSet GetArithmeticToken(string expression)
        {
            TokenSet result = ExpressionTokenizer.GetArithmeticToken(expression);
            return result;
            // TODO: add assertions to method ExpressionTokenizerTests.GetArithmeticToken(String)
        }

        /// <summary>Test stub for GetFunctionTokens(String)</summary>
        [PexMethod]
        internal TokenSet GetFunctionTokens(string filter)
        {
            TokenSet result = ExpressionTokenizer.GetFunctionTokens(filter);
            return result;
            // TODO: add assertions to method ExpressionTokenizerTests.GetFunctionTokens(String)
        }

        /// <summary>Test stub for GetTokens(String)</summary>
        [PexMethod]
        internal IEnumerable<TokenSet> GetTokens(string expression)
        {
            IEnumerable<TokenSet> result = ExpressionTokenizer.GetTokens(expression);
            return result;
            // TODO: add assertions to method ExpressionTokenizerTests.GetTokens(String)
        }
    }
}
