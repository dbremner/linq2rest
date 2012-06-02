// <copyright file="RestQueryableProviderBaseTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for RestQueryableProviderBase</summary>
    [PexClass(typeof(RestQueryableProviderBase))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestQueryableProviderBaseTests
    {
        /// <summary>Test stub for CreateQuery(Expression)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IQbservable<TResult> CreateQuery<TResult>(
            [PexAssumeNotNull]RestQueryableProviderBase target,
            Expression expression
        )
        {
            IQbservable<TResult> result = target.CreateQuery<TResult>(expression);
            return result;
            // TODO: add assertions to method RestQueryableProviderBaseTests.CreateQuery(RestQueryableProviderBase, Expression)
        }
    }
}
