// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Fakes
{
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Json;
	using Linq2Rest.Provider;

	public class TestSerializer<T> : ISerializer<T>
	{
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(T));
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<T>));

		public T Deserialize(Stream input)
		{
			return (T)_innerSerializer.ReadObject(input);
		}

		public IEnumerable<T> DeserializeList(Stream input)
		{
			return (List<T>)_innerListSerializer.ReadObject(input);
		}
	}
}