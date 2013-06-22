// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneSerializerFactory.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the PhoneSerializerFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive.WP8.Sample
{
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Json;
	using Provider;

	public class PhoneSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new PhoneSerializer<T>();
		}

		private class PhoneSerializer<T> : ISerializer<T>
		{
			private readonly DataContractJsonSerializer _innerListSerializer;
			private readonly DataContractJsonSerializer _innerSerializer;

			public PhoneSerializer()
			{
				_innerSerializer = new DataContractJsonSerializer(typeof(T));
				_innerListSerializer = new DataContractJsonSerializer(typeof(List<T>));
			}

			public T Deserialize(Stream input)
			{
					return (T)_innerSerializer.ReadObject(input);
			}

			public IEnumerable<T> DeserializeList(Stream input)
			{
					return (IEnumerable<T>)_innerListSerializer.ReadObject(input);
			}

            public Stream Serialize(T item)
            {
                var ms = new MemoryStream();
                _innerSerializer.WriteObject(ms, item);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
		}
	}
}