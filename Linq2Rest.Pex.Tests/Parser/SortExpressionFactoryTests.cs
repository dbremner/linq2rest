// <copyright file="SortExpressionFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for SortExpressionFactory</summary>
    [PexClass(typeof(SortExpressionFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class SortExpressionFactoryTests
    {
        /// <summary>Test stub for Create(String)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IEnumerable<SortDescription<T>> Create<T>([PexAssumeUnderTest]SortExpressionFactory target, string filter)
        {
            IEnumerable<SortDescription<T>> result = target.Create<T>(filter);
            return result;
            // TODO: add assertions to method SortExpressionFactoryTests.Create(SortExpressionFactory, String)
        }
    }
}
