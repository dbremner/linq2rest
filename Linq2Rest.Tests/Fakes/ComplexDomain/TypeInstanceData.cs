﻿using System;

namespace Linq2Rest.Tests.Fakes.ComplexDomain
{
	/// <summary>
	/// A type instance data 
	/// </summary>
	public class TypeInstanceData
	{
		#region Properties

		/// <summary>
		/// The unique id of this object, given by the user who created it
		/// </summary>

		public string UserDefinedId { get; set; }

		/// <summary>
		/// The last changed time 
		/// </summary>

		public DateTime LastChangedDate { get; set; }

		/// <summary>
		/// The full name of the type definition that this instance belongs to
		/// Full name includes all hierarchies (for Example: CPU\SNB\SNB2Core1Gfx)
		/// </summary>

		public string DefinitionFullName { get; set; }

		/// <summary>
		/// The properties that belong to this type instance
		/// </summary>

		public PropertyInstanceData[] Properties { get; set; }

		/// <summary>
		/// Determines whether this type instance was soft deleted
		/// </summary>

		public bool IsDeleted { get; set; }

		/// <summary>
		/// The name of the user which last updated this object 
		/// </summary>

		public string LastUpdatedBy { get; set; }

		#endregion
	}
}