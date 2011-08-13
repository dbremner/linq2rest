namespace UrlQueryParser.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;

	public class RestContext<T>
	{
		private readonly RestQueryable<T> _queryable;

		public RestContext(Uri serviceBase)
		{
			_queryable = new RestQueryable<T>(serviceBase);
		}

		public IQueryable<T> Query
		{
			get { return _queryable; }
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_queryable != null);
		}
	}
}