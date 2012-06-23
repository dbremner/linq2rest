// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Implementations
{
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	[DataContract]
	public class SimpleContractItem
	{
		[DataMember]
		public int Value { get; set; }

		[XmlElement(ElementName = "Text")]
		[DataMember(Name = "Text")]
		public string SomeString { get; set; }
	}
}