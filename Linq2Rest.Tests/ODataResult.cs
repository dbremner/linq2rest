// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;

	public class ODataResult<T>
	{
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Needed for test.")]
		public List<T> results { get; set; }
	}
}