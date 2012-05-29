// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Specialized;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// Defines the public interface for a parameter parser.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object to parse parameters for.</typeparam>
	[ContractClass(typeof(ParameterParserContracts<>))]
	public interface IParameterParser<in T>
	{
		/// <summary>
		/// Parses the passes parameters into a <see cref="ModelFilter{T}"/>.
		/// </summary>
		/// <param name="queryParameters">The parameters to parse.</param>
		/// <returns>A <see cref="ModelFilter{T}"/> representing the restrictions in the parameters.</returns>
		IModelFilter<T> Parse(NameValueCollection queryParameters);
	}

	[ContractClassFor(typeof(IParameterParser<>))]
	internal abstract class ParameterParserContracts<T> : IParameterParser<T>
	{
		public IModelFilter<T> Parse(NameValueCollection queryParameters)
		{
			Contract.Requires<ArgumentNullException>(queryParameters != null);
			throw new System.NotImplementedException();
		}
	}
}