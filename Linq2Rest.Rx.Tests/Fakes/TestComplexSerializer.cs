// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx.Tests.Fakes
{
	using System.Collections.Generic;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;

	public class TestComplexSerializer : ISerializer<FakeItem>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public FakeItem Deserialize(string input)
		{
			return _innerSerializer.Deserialize<FakeItem>(input);
		}

		public IList<FakeItem> DeserializeList(string input)
		{
			return _innerSerializer.Deserialize<List<FakeItem>>(input);
		}
	}
}