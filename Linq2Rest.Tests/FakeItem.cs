// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public class FakeItem
	{
		public int IntValue { get; set; }

		public double DoubleValue { get; set; }

		public decimal DecimalValue { get; set; }

		public string StringValue { get; set; }

		[DataMember(Name = "Date")]
		public DateTime DateValue { get; set; }

		public Choice ChoiceValue { get; set; }
	}
}
