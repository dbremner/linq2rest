namespace Linq2Rest.Tests.Fakes.ComplexDomain
{
	/// <summary>
	/// Defines a value type in Metal
	/// </summary>
	public enum ValueTypeDefinitionData : short
	{
		Reference = 0, // Reference to another TypeDefinitionData
		DateTime = 1,
		Bool = 2,
		Float = 3,
		Int = 4,
		StringNonUnicode = 5
	}
}