// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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