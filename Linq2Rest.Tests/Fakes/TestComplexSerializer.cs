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
	using Linq2Rest.Tests.Provider;

	public class TestComplexSerializer : ISerializer<ComplexDto>
	{
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(ComplexDto));
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<ComplexDto>));

		public ComplexDto Deserialize(Stream input)
		{
			return (ComplexDto)_innerSerializer.ReadObject(input);
		}

		public IEnumerable<ComplexDto> DeserializeList(Stream input)
		{
			return (List<ComplexDto>)_innerListSerializer.ReadObject(input);
		}
	}
}