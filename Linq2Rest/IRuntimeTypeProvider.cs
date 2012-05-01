// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Reflection;

	/// <summary>
	/// Provides a type matching the provided members.
	/// </summary>
	[ContractClass(typeof(RuntimeTypeProviderContracts))]
	public interface IRuntimeTypeProvider
	{
		/// <summary>
		/// Gets the <see cref="Type"/> matching the provided members.
		/// </summary>
		/// <param name="sourceType">The <see cref="Type"/> to generate the runtime type from.</param>
		/// <param name="properties">The <see cref="MemberInfo"/> to use to generate properties.</param>
		/// <returns>A <see cref="Type"/> mathing the provided properties.</returns>
		Type Get(Type sourceType, IEnumerable<MemberInfo> properties);
	}

	[ContractClassFor(typeof(IRuntimeTypeProvider))]
	internal abstract class RuntimeTypeProviderContracts : IRuntimeTypeProvider
	{
		public Type Get(Type sourceType, IEnumerable<MemberInfo> properties)
		{
			Contract.Requires<ArgumentNullException>(sourceType != null);
			Contract.Requires<ArgumentNullException>(properties != null);

			throw new NotImplementedException();
		}
	}
}