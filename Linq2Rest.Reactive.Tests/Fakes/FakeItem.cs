﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using System;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	[DataContract]
	public class FakeItem
	{
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
	}
}