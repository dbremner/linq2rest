// <copyright file="RestQueryProviderBaseTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq;
using System.Linq.Expressions;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
	/// <summary>This class contains parameterized unit tests for RestQueryProviderBase</summary>
	[PexClass(typeof(RestQueryProviderBase))]
	[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
	[PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
	[TestFixture]
	public partial class RestQueryProviderBaseTests
	{
		/// <summary>Test stub for CreateQuery(Expression)</summary>
		[PexMethod]
		internal IQueryable CreateQuery(
			[PexAssumeNotNull]RestQueryProviderBase target,
			Expression expression
		)
		{
			IQueryable result = target.CreateQuery(expression);
			return result;
			// TODO: add assertions to method RestQueryProviderBaseTests.CreateQuery(RestQueryProviderBase, Expression)
		}

		/// <summary>Test stub for CreateQuery(Expression)</summary>
		[PexGenericArguments(typeof(int))]
		[PexMethod]
		internal IQueryable<TElement> CreateQuery01<TElement>(
			[PexAssumeNotNull]RestQueryProviderBase target,
			Expression expression)
		{
			IQueryable<TElement> result = target.CreateQuery<TElement>(expression);
			return result;
			// TODO: add assertions to method RestQueryProviderBaseTests.CreateQuery01(RestQueryProviderBase, Expression)
		}

		/// <summary>Test stub for Dispose()</summary>
		[PexMethod]
		internal void Dispose([PexAssumeNotNull]RestQueryProviderBase target)
		{
			target.Dispose();
			// TODO: add assertions to method RestQueryProviderBaseTests.Dispose(RestQueryProviderBase)
		}

		/// <summary>Test stub for Execute(Expression)</summary>
		[PexMethod]
		internal object Execute(
			[PexAssumeNotNull]RestQueryProviderBase target,
			Expression expression
		)
		{
			object result = target.Execute(expression);
			return result;
			// TODO: add assertions to method RestQueryProviderBaseTests.Execute(RestQueryProviderBase, Expression)
		}

		/// <summary>Test stub for Execute(Expression)</summary>
		[PexGenericArguments(typeof(int))]
		[PexMethod]
		internal TResult Execute01<TResult>(
			[PexAssumeNotNull]RestQueryProviderBase target,
			Expression expression
		)
		{
			TResult result = target.Execute<TResult>(expression);
			return result;
			// TODO: add assertions to method RestQueryProviderBaseTests.Execute01(RestQueryProviderBase, Expression)
		}
	}
}
