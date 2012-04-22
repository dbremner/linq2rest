// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.WP7Sample
{
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Json;
	using Linq2Rest.Provider;

	public class PhoneSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new PhoneSerializer<T>();
		}

		private class PhoneSerializer<T> : ISerializer<T>
		{
			private readonly DataContractJsonSerializer _innerSerializer;
			private readonly DataContractJsonSerializer _innerListSerializer;

			public PhoneSerializer()
			{
				_innerSerializer = new DataContractJsonSerializer(typeof(T));
				_innerListSerializer = new DataContractJsonSerializer(typeof(List<T>));
			}

			public T Deserialize(Stream input)
			{
					return (T)_innerSerializer.ReadObject(input);
			}

			public IList<T> DeserializeList(Stream input)
			{
					return (IList<T>)_innerListSerializer.ReadObject(input);
			}
		}
	}
}