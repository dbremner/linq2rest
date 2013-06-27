// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestQueryProviderBase.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestQueryProviderBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	internal abstract class RestQueryProviderBase : IQueryProvider, IDisposable
	{
		private static readonly MethodInfo InnerCreateMethod = typeof(ISerializerFactory).GetMethod("Create");

		protected static MethodInfo CreateMethod
		{
			get
			{
				return InnerCreateMethod;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public abstract IQueryable CreateQuery(Expression expression);

		public abstract IQueryable<TElement> CreateQuery<TElement>(Expression expression);

		public abstract object Execute(Expression expression);

		public TResult Execute<TResult>(Expression expression)
		{
			Contract.Assume(expression != null);
			return (TResult)Execute(expression);
		}

		protected abstract void Dispose(bool disposing);
	}
}