// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Runtime.Serialization;

namespace Linq2Rest.Reactive.WinRT.Sample.Support
{
	[DataContract]
	public class ODataResponse<T>
	{
		[DataMember(Name = "d")]
		public ODataResult<T> Result { get; set; }
	}
}