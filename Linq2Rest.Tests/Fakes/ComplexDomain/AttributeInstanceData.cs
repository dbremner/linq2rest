namespace Linq2Rest.Tests.Fakes.ComplexDomain
{
	/// <summary>
	/// An attribute instance data 
	/// </summary>
	public class AttributeInstanceData
	{
		/// <summary>
		/// The attribue name 
		/// </summary>
		public string DefinitionName { get; set; }

		/// <summary>
		/// The parameters added to this attribute  
		/// </summary>
		public string Params { get; set; }

		/// <summary>
		/// Determines whether this attribue is inherited from a base type
		/// </summary>
		public bool IsInherited { get; set; }

	}
}