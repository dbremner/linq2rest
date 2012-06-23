// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Linq2Rest.Provider;

namespace Linq2Rest.Reactive.WinRT.Tests.Fakes
{
	public class TestSerializer<T> : ISerializer<T>
	{
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(T));
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<T>));

		public T Deserialize(string input)
		{
			return default(T);
		}

		public IList<T> DeserializeList(string input)
		{
			return new List<T>();
		}
	}
}