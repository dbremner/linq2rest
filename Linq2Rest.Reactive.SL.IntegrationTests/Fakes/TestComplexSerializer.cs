// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestComplexSerializer.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TestComplexSerializer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive.SL.IntegrationTests.Fakes
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Serialization.Json;
	using Linq2Rest.Provider;

	public class TestComplexSerializer : ISerializer<FakeItem>
	{
		private readonly DataContractJsonSerializer _innerListSerializer = new DataContractJsonSerializer(typeof(List<FakeItem>));
		private readonly DataContractJsonSerializer _innerSerializer = new DataContractJsonSerializer(typeof(FakeItem));

		public FakeItem Deserialize(Stream input)
		{
			return (FakeItem)_innerSerializer.ReadObject(input);
		}

		public IEnumerable<FakeItem> DeserializeList(Stream input)
		{
			return (List<FakeItem>)_innerListSerializer.ReadObject(input);
		}

		/// <summary>
		/// Deserializes a single item.
		/// </summary>
		/// <param name="input">The serialized item.</param>
		/// <param name="sourceType">The <see cref="Type"/> which provides alias information.</param>
		/// <returns>An instance of the serialized item.</returns>
		public FakeItem Deserialize(Stream input, Type sourceType)
		{
			return Deserialize(input);
		}

		/// <summary>
		/// Deserializes a list of items.
		/// </summary>
		/// <param name="input">The serialized items.</param>
		/// <param name="sourceType">The <see cref="Type"/> which provides alias information.</param>
		/// <returns>An list of the serialized items.</returns>
		public IEnumerable<FakeItem> DeserializeList(Stream input, Type sourceType)
		{
			return DeserializeList(input);
		}

		public Stream Serialize(FakeItem item)
		{
			var stream = new MemoryStream();
			_innerSerializer.WriteObject(stream, item);
			stream.Flush();
			stream.Position = 0;

			return stream;
		}
	}
}