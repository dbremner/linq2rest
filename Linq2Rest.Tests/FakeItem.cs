// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	[DataContract]
	public class FakeItem
	{
		private readonly Collection<FakeChildItem> _children = new Collection<FakeChildItem>();
		[DataMember(Name = "Text")]
		private string _stringValue;

		public int ID { get; set; }

		public Guid GlobalID { get; set; }

		[XmlElement(ElementName = "Number")]
		public int IntValue { get; set; }

		public double DoubleValue { get; set; }

		public decimal DecimalValue { get; set; }

		public string StringValue
		{
			get
			{
				return _stringValue;
			}

			set
			{
				_stringValue = value;
			}
		}

		public DateTime DateValue { get; set; }

		public TimeSpan Duration { get; set; }

		public DateTimeOffset PointInTime { get; set; }

		[DataMember(Name = "Choice")]
		public Choice ChoiceValue { get; set; }

		public ICollection<FakeChildItem> Children
		{
			get { return _children; }
		}
	}
}
