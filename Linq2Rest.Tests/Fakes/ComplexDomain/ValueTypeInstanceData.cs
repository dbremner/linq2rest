using System;

namespace Linq2Rest.Tests.Fakes.ComplexDomain
{
	/// <summary>
	/// A value type instance data 
	/// </summary>
	public class ValueTypeInstanceData
	{
		/// <summary>
		/// The value type
		/// </summary>

		public ValueTypeDefinitionData ValueType { get; set; }

		/// <summary>
		/// The DateTime value 
		/// </summary>

		public DateTime? DateTimeValue { get; set; }

		/// <summary>
		/// The bool value 
		/// </summary>

		public bool? BoolValue { get; set; }

		/// <summary>
		/// The float value 
		/// </summary>

		public float? FloatValue { get; set; }

		/// <summary>
		/// The int value 
		/// </summary>

		public int? IntValue { get; set; }

		/// <summary>
		/// The string value 
		/// </summary>

		public string StringNonUnicodeValue { get; set; }
	}
}