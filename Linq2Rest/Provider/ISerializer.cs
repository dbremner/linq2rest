// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System.Collections.Generic;

	public interface ISerializer<T>
	{
		T Deserialize(string input);

		IList<T> DeserializeList(string input);
	}

	public interface ISerializerFactory
	{
		ISerializer<T> Create<T>();
	}
}