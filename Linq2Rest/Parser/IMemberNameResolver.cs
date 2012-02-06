// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System.Reflection;

	public interface IMemberNameResolver
	{
		string ResolveName(MemberInfo member);
	}
}