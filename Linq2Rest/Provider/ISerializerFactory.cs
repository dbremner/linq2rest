// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif

	/// <summary>
	/// Defines the public interface for a factory of <see cref="ISerializer{T}"/>.
	/// </summary>
#if !WINDOWS_PHONE
	[ContractClass(typeof(SerializerFactoryContracts))]
#endif
	public interface ISerializerFactory
	{
		/// <summary>
		/// Creates an instance of an <see cref="ISerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The item type for the serializer.</typeparam>
		/// <returns>An instance of an <see cref="ISerializer{T}"/>.</returns>
		ISerializer<T> Create<T>();
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(ISerializerFactory))]
	internal abstract class SerializerFactoryContracts : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			Contract.Ensures(Contract.Result<ISerializer<T>>() != null);
			throw new System.NotImplementedException();
		}
	}
#endif
}