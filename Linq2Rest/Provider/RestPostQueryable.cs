namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq.Expressions;

	internal class RestPostQueryable<T> : RestQueryableBase<T>
	{
		private readonly RestPostQueryProvider<T> _restPostQueryProvider;

		public RestPostQueryable(IRestClient client, ISerializerFactory serializerFactory, Expression expression, Stream inputData)
			: base(client, serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expression != null);

			_restPostQueryProvider = new RestPostQueryProvider<T>(
				client,
				serializerFactory, 
				new ExpressionProcessor(new ExpressionWriter()),
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