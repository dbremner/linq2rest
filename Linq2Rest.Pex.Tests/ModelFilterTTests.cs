// <copyright file="ModelFilterTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Linq2Rest;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest
{
    /// <summary>This class contains parameterized unit tests for ModelFilter`1</summary>
    [PexClass(typeof(ModelFilter<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ModelFilterTTests
    {
        /// <summary>Test stub for .ctor(Expression`1&lt;Func`2&lt;!0,Boolean&gt;&gt;, Expression`1&lt;Func`2&lt;!0,Object&gt;&gt;, IEnumerable`1&lt;SortDescription`1&lt;!0&gt;&gt;, Int32, Int32)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal ModelFilter<T> Constructor<T>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>> selectExpression,
            IEnumerable<SortDescription<T>> sortDescriptions,
            int skip,
            int top
        )
        {
            ModelFilter<T> target = new ModelFilter<T>
                                        (filterExpression, selectExpression, sortDescriptions, skip, top);
            return target;
            // TODO: add assertions to method ModelFilterTTests.Constructor(Expression`1<Func`2<!!0,Boolean>>, Expression`1<Func`2<!!0,Object>>, IEnumerable`1<SortDescription`1<!!0>>, Int32, Int32)
        }

        /// <summary>Test stub for Filter(IEnumerable`1&lt;!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        internal IEnumerable<object> Filter<T>(
            [PexAssumeUnderTest]ModelFilter<T> target,
            IEnumerable<T> model
        )
        {
            IEnumerable<object> result = target.Filter(model);
            return result;
            // TODO: add assertions to method ModelFilterTTests.Filter(ModelFilter`1<!!0>, IEnumerable`1<!!0>)
        }
    }
}
