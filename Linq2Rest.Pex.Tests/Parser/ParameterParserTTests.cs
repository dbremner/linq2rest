// <copyright file="ParameterParserTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Specialized;
using Linq2Rest;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for ParameterParser`1</summary>
    [PexClass(typeof(ParameterParser<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ParameterParserTTests
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public ParameterParser<T> Constructor<T>()
        {
            ParameterParser<T> target = new ParameterParser<T>();
            return target;
            // TODO: add assertions to method ParameterParserTTests.Constructor()
        }

        /// <summary>Test stub for .ctor(IFilterExpressionFactory, ISortExpressionFactory, ISelectExpressionFactory`1&lt;!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public ParameterParser<T> Constructor01<T>(
            IFilterExpressionFactory filterExpressionFactory,
            ISortExpressionFactory sortExpressionFactory,
            ISelectExpressionFactory<T> selectExpressionFactory
        )
        {
            ParameterParser<T> target = new ParameterParser<T>
                                            (filterExpressionFactory, sortExpressionFactory, selectExpressionFactory);
            return target;
            // TODO: add assertions to method ParameterParserTTests.Constructor01(IFilterExpressionFactory, ISortExpressionFactory, ISelectExpressionFactory`1<!!0>)
        }

        /// <summary>Test stub for Parse(NameValueCollection)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IModelFilter<T> Parse<T>(
            [PexAssumeUnderTest]ParameterParser<T> target,
            NameValueCollection queryParameters
        )
        {
            IModelFilter<T> result = target.Parse(queryParameters);
            return result;
            // TODO: add assertions to method ParameterParserTTests.Parse(ParameterParser`1<!!0>, NameValueCollection)
        }
    }
}
