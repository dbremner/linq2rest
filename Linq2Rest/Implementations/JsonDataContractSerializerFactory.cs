// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Implementations
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization.Json;
	using System.Text;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines the JsonDataContractSerializer factory.
	/// </summary>
	public class JsonDataContractSerializerFactory : ISerializerFactory
	{
		private readonly IEnumerable<Type> _knownTypes;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonDataContractSerializerFactory"/> class.
		/// </summary>
		/// <param name="knownTypes">A number of known types for serialization resolution.</param>
		public JsonDataContractSerializerFactory(IEnumerable<Type> knownTypes)
		{
			Contract.Requires<ArgumentNullException>(knownTypes != null);

			_knownTypes = knownTypes;
		}

		/// <summary>
		/// Creates an instance of an <see cref="ISerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The item type for the serializer.</typeparam>
		/// <returns>An instance of an <see cref="ISerializer{T}"/>.</returns>
		public ISerializer<T> Create<T>()
		{
			return new JsonDataContractSerializer<T>(_knownTypes);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_knownTypes != null);
		}

		private class JsonDataContractSerializer<T> : ISerializer<T>
		{
			private readonly DataContractJsonSerializer _serializer;
			private readonly DataContractJsonSerializer _listSerializer;

			public JsonDataContractSerializer(IEnumerable<Type> knownTypes)
			{
				Contract.Requires(knownTypes != null);

				var array = knownTypes.ToArray();
				_serializer = new DataContractJsonSerializer(typeof(T), array);
				_listSerializer = new DataContractJsonSerializer(typeof(List<T>), array);
			}

			public T Deserialize(string input)
			{
				using (var ms = new MemoryStream(Encoding.Default.GetBytes(input)))
				{
					ms.Seek(0, SeekOrigin.Begin);
					var result = (T)_serializer.ReadObject(ms);

					return result;
				}
			}

			public IList<T> DeserializeList(string input)
			{
				using (var ms = new MemoryStream(Encoding.Default.GetBytes(input)))
				{
					ms.Seek(0, SeekOrigin.Begin);
					var result = (List<T>)_listSerializer.ReadObject(ms);

					return result;
				}
			}

			[ContractInvariantMethod]
			private void Invariants()
			{
				Contract.Invariant(_serializer != null);
				Contract.Invariant(_listSerializer != null);
			}
		}
	}
}