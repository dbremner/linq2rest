// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

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