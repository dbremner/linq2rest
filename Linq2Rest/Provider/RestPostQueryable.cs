// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestPostQueryable.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestPostQueryable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq.Expressions;
	using Linq2Rest.Parser;

	internal class RestPostQueryable<T> : RestQueryableBase<T>
	{
		private readonly RestPostQueryProvider<T> _restPostQueryProvider;

		public RestPostQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression, Stream inputData)
			: this(client, serializerFactory, new MemberNameResolver(), expression, inputData)
		{
		}

		public RestPostQueryable(IRestClient client, ISerializerFactory serializerFactory, IMemberNameResolver memberNameResolver, Expression expression, Stream inputData)
			: base(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expression != null);

			_restPostQueryProvider = new RestPostQueryProvider<T>(
				client,
				serializerFactory,
				new ExpressionProcessor(new ExpressionWriter(memberNameResolver), memberNameResolver),
				inputData);
			Provider = _restPostQueryProvider;
			Expression = expression;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					_restPostQueryProvider.Dispose();
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
			Contract.Invariant(_restPostQueryProvider != null);
		}
	}
}