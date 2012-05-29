// <copyright file="QueryableExtensionsTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
    /// <summary>This class contains parameterized unit tests for QueryableExtensions</summary>
    [PexClass(typeof(QueryableExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class QueryableExtensionsTests
    {
        /// <summary>Test stub for ExecuteAsync(IQueryable`1&lt;!!0&gt;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public Task<IEnumerable<T>> ExecuteAsync<T>(IQueryable<T> queryable)
        {
            Task<IEnumerable<T>> result = QueryableExtensions.ExecuteAsync<T>(queryable);
            return result;
            // TODO: add assertions to method QueryableExtensionsTests.ExecuteAsync(IQueryable`1<!!0>)
        }

        /// <summary>Test stub for Expand(IQueryable`1&lt;!!0&gt;, String)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQueryable<TSource> Expand<TSource>(IQueryable<TSource> source, string paths)
        {
            IQueryable<TSource> result = QueryableExtensions.Expand<TSource>(source, paths);
            return result;
            // TODO: add assertions to method QueryableExtensionsTests.Expand(IQueryable`1<!!0>, String)
        }

        /// <summary>Test stub for Expand(IQueryable`1&lt;!!0&gt;, Expression`1&lt;Func`2&lt;!!0,Object&gt;&gt;[])</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IQueryable<TSource> Expand01<TSource>(IQueryable<TSource> source, Expression<Func<TSource, object>>[] properties)
        {
            IQueryable<TSource> result
               = QueryableExtensions.Expand<TSource>(source, properties);
            return result;
            // TODO: add assertions to method QueryableExtensionsTests.Expand01(IQueryable`1<!!0>, Expression`1<Func`2<!!0,Object>>[])
        }
    }
}
