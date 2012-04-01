// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Rx.Tests.Fakes
{
	using System.Diagnostics.CodeAnalysis;

	public class ODataResponse<T>
	{
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Needed for test.")]
		public ODataResult<T> d { get; set; }
	}
}