// <copyright file="RestQueryProviderTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for RestQueryProvider`1</summary>
    [PexClass(typeof(RestQueryProvider<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestQueryProviderTTests
    {
        /// <summary>Test stub for .ctor(IRestClient, ISerializerFactory)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal RestQueryProvider<T> Constructor<T>(IRestClient client, ISerializerFactory serializerFactory)
        {
            RestQueryProvider<T> target
               = new RestQueryProvider<T>(client, serializerFactory);
            return target;
            // TODO: add assertions to method RestQueryProviderTTests.Constructor(IRestClient, ISerializerFactory)
        }

        /// <summary>Test stub for .ctor(IRestClient, ISerializerFactory, IExpressionProcessor)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal RestQueryProvider<T> Constructor01<T>(
            IRestClient client,
            ISerializerFactory serializerFactory,
            IExpressionProcessor expressionProcessor
        )
        {
            RestQueryProvider<T> target
               = new RestQueryProvider<T>(client, serializerFactory, expressionProcessor);
            return target;
            // TODO: add assertions to method RestQueryProviderTTests.Constructor01(IRestClient, ISerializerFactory, IExpressionProcessor)
        }

        /// <summary>Test stub for CreateQuery(Expression)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQueryable CreateQuery<T>(
            [PexAssumeUnderTest]RestQueryProvider<T> target,
            Expression expression
        )
        {
            IQueryable result = target.CreateQuery(expression);
            return result;
            // TODO: add assertions to method RestQueryProviderTTests.CreateQuery(RestQueryProvider`1<!!0>, Expression)
        }

        /// <summary>Test stub for CreateQuery(Expression)</summary>
        [PexGenericArguments(typeof(int), typeof(int))]
        [PexMethod]
        internal IQueryable<TResult> CreateQuery01<T,TResult>(
            [PexAssumeUnderTest]RestQueryProvider<T> target,
            Expression expression
        )
        {
            IQueryable<TResult> result = target.CreateQuery<TResult>(expression);
            return result;
            // TODO: add assertions to method RestQueryProviderTTests.CreateQuery01(RestQueryProvider`1<!!0>, Expression)
        }

        /// <summary>Test stub for Execute(Expression)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal object Execute<T>(
            [PexAssumeUnderTest]RestQueryProvider<T> target,
            Expression expression
        )
        {
            object result = target.Execute(expression);
            return result;
            // TODO: add assertions to method RestQueryProviderTTests.Execute(RestQueryProvider`1<!!0>, Expression)
        }

        /// <summary>Test stub for Execute(Expression)</summary>
        [PexGenericArguments(typeof(int), typeof(int))]
        [PexMethod]
        internal TResult Execute01<T,TResult>(
            [PexAssumeUnderTest]RestQueryProvider<T> target,
            Expression expression
        )
        {
            TResult result = target.Execute<TResult>(expression);
            return result;
            // TODO: add assertions to method RestQueryProviderTTests.Execute01(RestQueryProvider`1<!!0>, Expression)
        }
    }
}
