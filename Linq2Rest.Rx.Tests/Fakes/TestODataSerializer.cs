// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx.Tests.Fakes
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;

	public class TestODataSerializer<T> : ISerializer<T>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public T Deserialize(string input)
		{
			var response = _innerSerializer.Deserialize<ODataResponse<T>>(input);
			return response.d.results.FirstOrDefault();
		}

		public IList<T> DeserializeList(string input)
		{
			var list = _innerSerializer.Deserialize<ODataResponse<T>>(input);
			var items = list.d.results;

			return items;
		}
	}
}