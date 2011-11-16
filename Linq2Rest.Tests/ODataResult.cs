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