// <copyright file="AsyncExpressionProcessorTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for AsyncExpressionProcessor</summary>
    [PexClass(typeof(AsyncExpressionProcessor))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class AsyncExpressionProcessorTests
    {
        /// <summary>Test stub for .ctor(IExpressionWriter)</summary>
        [PexMethod(MaxConditions = 1000)]
        internal AsyncExpressionProcessor Constructor(IExpressionWriter writer)
        {
            AsyncExpressionProcessor target = new AsyncExpressionProcessor(writer);
            return target;
            // TODO: add assertions to method AsyncExpressionProcessorTests.Constructor(IExpressionWriter)
        }

        /// <summary>Test stub for ProcessMethodCall(MethodCallExpression, ParameterBuilder, Func`2&lt;ParameterBuilder,IObservable`1&lt;IEnumerable`1&lt;!!0&gt;&gt;&gt;, Func`3&lt;Type,ParameterBuilder,IObservable`1&lt;IEnumerable&gt;&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IObservable<T> ProcessMethodCall<T>(
            [PexAssumeUnderTest]AsyncExpressionProcessor target,
            MethodCallExpression methodCall,
            ParameterBuilder builder,
            Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader,
            Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader
        )
        {
            IObservable<T> result = target.ProcessMethodCall<T>
                                        (methodCall, builder, resultLoader, intermediateResultLoader);
            return result;
            // TODO: add assertions to method AsyncExpressionProcessorTests.ProcessMethodCall(AsyncExpressionProcessor, MethodCallExpression, ParameterBuilder, Func`2<ParameterBuilder,IObservable`1<IEnumerable`1<!!0>>>, Func`3<Type,ParameterBuilder,IObservable`1<IEnumerable>>)
        }
    }
}
