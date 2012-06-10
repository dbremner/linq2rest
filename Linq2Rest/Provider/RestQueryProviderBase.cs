// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
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

		public abstract IQueryable CreateQuery(Expression expression);

		public abstract IQueryable<TElement> CreateQuery<TElement>(Expression expression);

		public abstract object Execute(Expression expression);

		public abstract TResult Execute<TResult>(Expression expression);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected abstract void Dispose(bool disposing);
	}
}