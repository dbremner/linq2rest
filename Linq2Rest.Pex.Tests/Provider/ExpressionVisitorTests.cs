// <copyright file="ExpressionVisitorTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for ExpressionWriter</summary>
    [PexClass(typeof(ExpressionWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ExpressionVisitorTests
    {
        /// <summary>Test stub for Visit(Expression)</summary>
        [PexMethod]
        internal string Visit(
            [PexAssumeUnderTest]ExpressionWriter target,
            Expression expression
        )
        {
            string result = target.Visit(expression);
            return result;
            // TODO: add assertions to method ExpressionVisitorTests.Visit(ExpressionWriter, Expression)
        }
    }
}
