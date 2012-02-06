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
	using System.Xml;
	using System.Xml.Serialization;
	using Linq2Rest.Provider;

	public class XmlSerializerFactory : ISerializerFactory
	{
		private readonly IEnumerable<Type> _knownTypes;

		public XmlSerializerFactory(IEnumerable<Type> knownTypes)
		{
			Contract.Requires<ArgumentNullException>(knownTypes != null);

			_knownTypes = knownTypes;
		}

		public ISerializer<T> Create<T>()
		{
			return new XmlSerializer<T>(_knownTypes);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_knownTypes != null);
		}

		private class XmlSerializer<T> : ISerializer<T>
		{
			private readonly XmlSerializer _serializer;
			private readonly XmlSerializer _listSerializer;

			public XmlSerializer(IEnumerable<Type> knownTypes)
			{
				var array = knownTypes.ToArray();
				_serializer = new XmlSerializer(typeof(T), array);
				_listSerializer = new XmlSerializer(typeof(List<T>), array);
			}

			public T Deserialize(string input)
			{
				var result = (T)_serializer.Deserialize(XmlReader.Create(new StringReader(input)));

				return result;
			}

			public IList<T> DeserializeList(string input)
			{
				var result = (List<T>)_listSerializer.Deserialize(XmlReader.Create(new StringReader(input)));

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