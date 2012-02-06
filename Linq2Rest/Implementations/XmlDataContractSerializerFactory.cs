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

	public class XmlDataContractSerializerFactory : ISerializerFactory
	{
		private readonly IEnumerable<Type> _knownTypes;

		public XmlDataContractSerializerFactory(IEnumerable<Type> knownTypes)
		{
			Contract.Requires<ArgumentNullException>(knownTypes != null);

			_knownTypes = knownTypes;
		}

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
				var array = knownTypes.ToArray();
				_serializer = new DataContractSerializer(typeof(T), array);
				_listSerializer = new DataContractSerializer(typeof(List<T>), array);
			}

			public T Deserialize(string input)
			{
				var result = (T)_serializer.ReadObject(XmlReader.Create(new StringReader(input)));

				return result;
			}

			public IList<T> DeserializeList(string input)
			{
				var result = (List<T>)_listSerializer.ReadObject(XmlReader.Create(new StringReader(input)));

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
