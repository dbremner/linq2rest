// <copyright file="FilterExpressionFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for FilterExpressionFactory</summary>
    [PexClass(typeof(FilterExpressionFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class FilterExpressionFactoryTests
    {
        /// <summary>Test stub for Create(String)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public Expression<Func<T, bool>> Create<T>(
            [PexAssumeUnderTest]FilterExpressionFactory target,
            string filter
        )
        {
            Expression<Func<T, bool>> result = target.Create<T>(filter);
            return result;
            // TODO: add assertions to method FilterExpressionFactoryTests.Create(FilterExpressionFactory, String)
        }

        /// <summary>Test stub for Create(String, IFormatProvider)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public Expression<Func<T, bool>> Create01<T>(
            [PexAssumeUnderTest]FilterExpressionFactory target,
            string filter,
            IFormatProvider formatProvider)
        {
            Expression<Func<T, bool>> result = target.Create<T>(filter, formatProvider);
            return result;
            // TODO: add assertions to method FilterExpressionFactoryTests.Create01(FilterExpressionFactory, String, IFormatProvider)
        }
    }
}
