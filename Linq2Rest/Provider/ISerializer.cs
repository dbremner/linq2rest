// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the public interface for an object serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	/// Defines the public interface for an object serializer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISerializer<T>
	{
		/// <summary>
		/// Deserializes a single item.
		/// </summary>
		/// <param name="input">The serialized item.</param>
		/// <returns>An instance of the serialized item.</returns>
		T Deserialize(Stream input);

		/// <summary>
		/// Deserializes a list of items.
		/// </summary>
		/// <param name="input">The serialized items.</param>
		/// <returns>An list of the serialized items.</returns>
		IEnumerable<T> DeserializeList(Stream input);

		/// <summary>
		/// Serializes the passed item into a <see cref="Stream"/>.
		/// </summary>
		/// <param name="item">The item to serialize.</param>
		/// <returns>A <see cref="Stream"/> representation of the item.</returns>
		Stream Serialize(T item);
	}
}