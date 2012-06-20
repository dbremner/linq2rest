// <copyright file="UnsignedIntExpressionFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using Linq2Rest.Parser.Readers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser.Readers
{
    /// <summary>This class contains parameterized unit tests for UnsignedIntExpressionFactory</summary>
    [PexClass(typeof(UnsignedIntExpressionFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class UnsignedIntExpressionFactoryTests
    {
        /// <summary>Test stub for Convert(String)</summary>
        [PexMethod]
        internal ConstantExpression Convert(
            [PexAssumeUnderTest]UnsignedIntExpressionFactory target,
            string token
        )
        {
            ConstantExpression result = target.Convert(token);
            return result;
            // TODO: add assertions to method UnsignedIntExpressionFactoryTests.Convert(UnsignedIntExpressionFactory, String)
        }

        /// <summary>Test stub for get_Handles()</summary>
        [PexMethod]
        internal Type HandlesGet([PexAssumeUnderTest]UnsignedIntExpressionFactory target)
        {
            Type result = target.Handles;
            return result;
            // TODO: add assertions to method UnsignedIntExpressionFactoryTests.HandlesGet(UnsignedIntExpressionFactory)
        }
    }
}
