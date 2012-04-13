// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Linq2Rest.Tests
{
	using System;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	[DataContract]
	public class FakeItem
	{
        public int ID { get; set; }

		[DataMember(Name = "Text")]
		private string _stringValue;

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

		[DataMember(Name = "Choice")]
		public Choice ChoiceValue { get; set; }

        private readonly Collection<FakeChildItem> children = new Collection<FakeChildItem>();

        public ICollection<FakeChildItem> Children{
            get { return children; }
        }
	}

    [DataContract]
    public class FakeChildItem {
        public int ID { get; set; }

        public string ChildStringValue { get; set; }

        private readonly Collection<FakeGrandChildItem> children = new Collection<FakeGrandChildItem>();

        public ICollection<FakeGrandChildItem> Children {
            get { return children; }
        }
    }

    [DataContract]
    public class FakeGrandChildItem {
        public string GrandChildStringValue { get; set; }
    }
}
