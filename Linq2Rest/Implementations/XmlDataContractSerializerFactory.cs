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
	using System.Runtime.Serialization;
	using System.Xml;
	using Linq2Rest.Provider;

	/// <summary>
	/// Defines the XmlDataContractSerializer factory.
	/// </summary>
	public class XmlDataContractSerializerFactory : ISerializerFactory
	{
		private readonly IEnumerable<Type> _knownTypes;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlDataContractSerializerFactory"/> class.
		/// </summary>
		/// <param name="knownTypes">A number of known types for serialization resolution.</param>
		public XmlDataContractSerializerFactory(IEnumerable<Type> knownTypes)
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
			return new XmlDataContractSerializer<T>(_knownTypes);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_knownTypes != null);
		}

		private class XmlDataContractSerializer<T> : ISerializer<T>
		{
			private readonly DataContractSerializer _serializer;
			private readonly DataContractSerializer _listSerializer;

			public XmlDataContractSerializer(IEnumerable<Type> knownTypes)
			{
				Contract.Requires(knownTypes != null);

				var array = knownTypes.ToArray();
				_serializer = new DataContractSerializer(typeof(T), array);
				_listSerializer = new DataContractSerializer(typeof(List<T>), array);
			}

			public T Deserialize(Stream input)
			{
				var result = (T)_serializer.ReadObject(XmlReader.Create(input));

				return result;
			}

			public IList<T> DeserializeList(Stream input)
			{
				var result = (List<T>)_listSerializer.ReadObject(XmlReader.Create(input));

				return result;
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
