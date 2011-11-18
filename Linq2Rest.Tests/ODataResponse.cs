namespace Linq2Rest.Tests
{
	using System.Diagnostics.CodeAnalysis;

	public class ODataResponse<T>
	{
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Needed for test.")]
		public ODataResult<T> d { get; set; }
	}
}