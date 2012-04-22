// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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
	}
}