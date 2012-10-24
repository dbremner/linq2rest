// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestGetQueryable.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestGetQueryable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;

	internal class RestGetQueryable<T> : RestQueryableBase<T>
	{
		private readonly RestGetQueryProvider<T> _restGetQueryProvider;

		public RestGetQueryable(IRestClient client, ISerializerFactory serializerFactory)
			: base(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_restGetQueryProvider = new RestGetQueryProvider<T>(client, serializerFactory, new ExpressionProcessor(new ExpressionWriter()));
			Provider = _restGetQueryProvider;
			Expression = Expression.Constant(this);
		}

		public RestGetQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression)
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
					_restGetQueryProvider.Dispose();
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
			Contract.Invariant(_restGetQueryProvider != null);
		}
	}
}