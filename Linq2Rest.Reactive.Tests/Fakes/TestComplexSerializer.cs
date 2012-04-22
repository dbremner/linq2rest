// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Json;
	using Linq2Rest.Provider;

	public class TestComplexSerializer : ISerializer<FakeItem>
	{
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(FakeItem));
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<FakeItem>));

		public FakeItem Deserialize(Stream input)
		{
			return (FakeItem)_innerSerializer.ReadObject(input);
		}

		public IList<FakeItem> DeserializeList(Stream input)
		{
			return (List<FakeItem>)_innerListSerializer.ReadObject(input);
		}
	}
}