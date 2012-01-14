// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System.Collections.Generic;
	using System.Web.Script.Serialization;

	using Linq2Rest.Provider;
	using Linq2Rest.Tests.Provider;

	public class TestSerializer : ISerializer<SimpleDto>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public SimpleDto Deserialize(string input)
		{
			return _innerSerializer.Deserialize<SimpleDto>(input);
		}

		public IList<SimpleDto> DeserializeList(string input)
		{
			return _innerSerializer.Deserialize<List<SimpleDto>>(input);
		}
	}
}