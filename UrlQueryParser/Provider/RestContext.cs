namespace UrlQueryParser.Provider
{
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Web.Script.Serialization;

	public class RestContext<T>
	{
		private readonly RestQueryable<T> _queryable;

		public RestContext(IRestClient client, JavaScriptSerializer serializer)
		{
			_queryable = new RestQueryable<T>(client, serializer);
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