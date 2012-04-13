// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System.Runtime.Serialization;

	[DataContract]
	public class SampleDto
	{
		[DataMember]
		public string Text { get; set; }
	}
}
