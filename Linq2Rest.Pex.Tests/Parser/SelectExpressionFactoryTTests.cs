// <copyright file="SelectExpressionFactoryTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using Linq2Rest;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for SelectExpressionFactory`1</summary>
    [PexClass(typeof(SelectExpressionFactory<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class SelectExpressionFactoryTTests
    {
        /// <summary>Test stub for .ctor(IMemberNameResolver, IRuntimeTypeProvider)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public SelectExpressionFactory<T> Constructor<T>(
            IMemberNameResolver nameResolver,
            IRuntimeTypeProvider runtimeTypeProvider
        )
        {
            SelectExpressionFactory<T> target
               = new SelectExpressionFactory<T>(nameResolver, runtimeTypeProvider);
            return target;
            // TODO: add assertions to method SelectExpressionFactoryTTests.Constructor(IMemberNameResolver, IRuntimeTypeProvider)
        }

        /// <summary>Test stub for Create(String)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public Expression<Func<T, object>> Create<T>(
            [PexAssumeUnderTest]SelectExpressionFactory<T> target,
            string selection
        )
        {
            Expression<Func<T, object>> result = target.Create(selection);
            return result;
            // TODO: add assertions to method SelectExpressionFactoryTTests.Create(SelectExpressionFactory`1<!!0>, String)
        }
    }
}
