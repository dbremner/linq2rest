// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleDto.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleDto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public class SimpleDto
	{
		[DataMember(IsRequired = false)]
		public int ID { get; set; }

		[DataMember]
		public string Content { get; set; }

		[DataMember]
		public double Value { get; set; }

		[DataMember(IsRequired = false)]
		public DateTime Date { get; set; }

		[DataMember(IsRequired = false)]
		public TimeSpan Duration { get; set; }

		[DataMember(IsRequired = false)]
		public DateTimeOffset PointInTime { get; set; }

		[DataMember(IsRequired = false)]
		public Choice Choice { get; set; }
	}
}