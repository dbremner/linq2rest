// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class ODataResult<T>
	{
		[DataMember(Name = "results")]
		public List<T> Results { get; set; }
	}
}