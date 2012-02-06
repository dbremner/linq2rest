// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public interface IRuntimeTypeFactory
	{
		Type Create(Type sourceType, IEnumerable<MemberInfo> properties);
	}
}