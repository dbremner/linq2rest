// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestDeleteQueryable.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestDeleteQueryable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;

	internal class RestDeleteQueryable<T> : RestQueryableBase<T>
	{
		private readonly RestDeleteQueryProvider<T> _restDeleteQueryProvider;

		public RestDeleteQueryable(IRestClient client, ISerializerFactory serializerFactory)
			: base(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_restDeleteQueryProvider = new RestDeleteQueryProvider<T>(client, serializerFactory, new ExpressionProcessor(new ExpressionWriter()));
			Provider = _restDeleteQueryProvider;
			Expression = Expression.Constant(this);
		}

		public RestDeleteQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression)
			: this(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expression != null);

			Expression = expression;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					_restDeleteQueryProvider.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_restDeleteQueryProvider != null);
		}
	}
}