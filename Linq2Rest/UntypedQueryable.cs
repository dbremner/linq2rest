// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UntypedQueryable.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the UntypedQueryable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	internal class UntypedQueryable<T> : IQueryable<object>
	{
		private readonly IQueryable<T> _source;
		private readonly Expression<Func<T, object>> _projection;

		public UntypedQueryable(IQueryable<T> source, Expression<Func<T, object>> projection)
		{
			_source = source;
			_projection = projection;
			Expression = source.Expression;
			Provider = source.Provider;
		}

		public Expression Expression { get; private set; }

		public Type ElementType
		{
			get { return typeof(T); }
		}

		public IQueryProvider Provider { get; private set; }

		public IEnumerator<object> GetEnumerator()
		{
			return (_projection == null
						? _source.ToArray().OfType<object>()
						: _source.Select(_projection))
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}