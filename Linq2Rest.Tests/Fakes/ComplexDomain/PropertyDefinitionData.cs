using System;

namespace Linq2Rest.Tests.Fakes.ComplexDomain
{
	/// <summary>
	/// A property definition data 
	/// </summary>
	public class PropertyDefinitionData
	{
		#region Properties

		/// <summary>
		/// The property definition name that this instance belongs to
		/// </summary>

		public string Name { get; set; }

		/// <summary>
		/// The last changed time 
		/// </summary>

		public DateTime LastChangedDate { get; set; }

		/// <summary>
		/// Determines whether the value is a single one
		/// </summary>

		public bool IsSingleValue { get; set; }

		/// <summary>
		/// Determines whether value for this property must be assigned only from one of the predefined values
		/// </summary>

		public bool IsPredefinedValuesEnforced { get; set; }

		/// <summary>
		/// A value for this property must be assigned 
		/// </summary>

		public bool IsValueMandatory { get; set; }

		/// <summary>
		/// Determines whether this property definition can be changed only by the system (actual T-SQL script) and not by anyone else
		/// </summary>

		public bool IsSystemDefinition { get; set; }

		/// <summary>
		/// The value type of this property definition
		/// </summary>

		public ValueTypeDefinitionData ValueType { get; set; }

		/// <summary>
		/// The FullName of the TypeDefinition that this property definition is referencing to 
		/// (if the value type of this property is a reference to another TypeDefinition)
		/// </summary>

		public string ReferenceTypeFullName { get; set; }

		/// <summary>
		/// Determines whether this property is inherited from a base type
		/// </summary>

		public bool IsInherited { get; set; }

		/// <summary>
		/// The default value of this property definition
		/// </summary>

		public ValueTypeInstanceData DefaultValue { get; set; }

		/// <summary>
		/// The predefined values for this property definition
		/// </summary>

		public ValueTypeInstanceData[] PredefinedValues { get; set; }

		/// <summary>
		/// The collection of custom attributes for this property definition
		/// </summary>

		public AttributeInstanceData[] CustomAttributes { get; set; }

		#endregion
	}
}