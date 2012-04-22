// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Reflection;

	/// <summary>
	/// Defines the public interface for a resolver of <see cref="MemberInfo"/> name.
	/// </summary>
	[ContractClass(typeof(MemberNameResolverContracts))]
	public interface IMemberNameResolver
	{
		/// <summary>
		/// Returns the resolved name for the <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="member">The <see cref="MemberInfo"/> to resolve the name of.</param>
		/// <returns>The resolved name.</returns>
		string ResolveName(MemberInfo member);
	}

	[ContractClassFor(typeof(IMemberNameResolver))]
	internal abstract class MemberNameResolverContracts : IMemberNameResolver
	{
		public string ResolveName(MemberInfo member)
		{
			Contract.Requires<ArgumentNullException>(member != null);
			Contract.Ensures(Contract.Result<string>() != null);

			throw new System.NotImplementedException();
		}
	}
}