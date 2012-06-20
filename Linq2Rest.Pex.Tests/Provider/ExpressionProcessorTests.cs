// <copyright file="ExpressionProcessorTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for ExpressionProcessor</summary>
    [PexClass(typeof(ExpressionProcessor))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ExpressionProcessorTests
    {
        /// <summary>Test stub for .ctor(IExpressionWriter)</summary>
        [PexMethod]
        internal ExpressionProcessor Constructor(IExpressionWriter writer)
        {
            ExpressionProcessor target = new ExpressionProcessor(writer);
            return target;
            // TODO: add assertions to method ExpressionProcessorTests.Constructor(IExpressionWriter)
        }

        /// <summary>Test stub for ProcessMethodCall(MethodCallExpression, ParameterBuilder, Func`2&lt;ParameterBuilder,IEnumerable`1&lt;!!0&gt;&gt;, Func`3&lt;Type,ParameterBuilder,IEnumerable&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal object ProcessMethodCall<T>(
            [PexAssumeUnderTest]ExpressionProcessor target,
            MethodCallExpression methodCall,
            ParameterBuilder builder,
            Func<ParameterBuilder, IEnumerable<T>> resultLoader,
            Func<Type, ParameterBuilder, IEnumerable> intermediateResultLoader
        )
        {
            object result = target.ProcessMethodCall<T>
                                (methodCall, builder, resultLoader, intermediateResultLoader);
            return result;
            // TODO: add assertions to method ExpressionProcessorTests.ProcessMethodCall(ExpressionProcessor, MethodCallExpression, ParameterBuilder, Func`2<ParameterBuilder,IEnumerable`1<!!0>>, Func`3<Type,ParameterBuilder,IEnumerable>)
        }
    }
}
