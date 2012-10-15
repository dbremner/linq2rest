// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Fakes
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization.Json;
	using Linq2Rest.Provider;

	public class TestODataSerializer<T> : ISerializer<T>
	{
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(ODataResponse<T>));

		public T Deserialize(Stream input)
		{
			var response = (ODataResponse<T>)_innerSerializer.ReadObject(input);
			return response.Result.Results.FirstOrDefault();
		}

		public IEnumerable<T> DeserializeList(Stream input)
		{
			var response = (ODataResponse<T>)_innerSerializer.ReadObject(input);
			return response.Result.Results;
		}

		public Stream Serialize(T item)
		{
			throw new System.NotImplementedException();
		}
	}
}