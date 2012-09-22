namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq.Expressions;

	internal class RestPutQueryable<T> : RestQueryableBase<T>
	{
		private readonly RestPutQueryProvider<T> _restPutQueryProvider;

		public RestPutQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression, Stream inputData)
			: base(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expression != null);

			_restPutQueryProvider = new RestPutQueryProvider<T>(
				client,
				serializerFactory, 
				new ExpressionProcessor(new ExpressionWriter()),
				inputData);
			Provider = _restPutQueryProvider;
			Expression = expression;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					_restPutQueryProvider.Dispose();
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
			Contract.Invariant(_restPutQueryProvider != null);
		}
	}
}