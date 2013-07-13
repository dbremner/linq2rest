// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSerializer.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TestSerializer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive.SL.IntegrationTests.Fakes
{
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Json;
	using Provider;

	public class TestSerializer<T> : ISerializer<T>
	{
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<T>));
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(T));

		public T Deserialize(Stream input)
		{
			return (T)_innerSerializer.ReadObject(input);
		}

		public IEnumerable<T> DeserializeList(Stream input)
		{
			return (List<T>)_innerListSerializer.ReadObject(input);
		}

		public Stream Serialize(T item)
		{
			var stream = new MemoryStream();
			_innerSerializer.WriteObject(stream, item);
			stream.Flush();
			stream.Position = 0;

			return stream;
		}
	}
}