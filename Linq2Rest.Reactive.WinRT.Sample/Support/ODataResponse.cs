// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataResponse.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ODataResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive.WinRT.Sample.Support
{
	using System.Runtime.Serialization;

	[DataContract]
	public class ODataResponse<T>
	{
		[DataMember(Name = "d")]
		public ODataResult<T> Result { get; set; }
	}
}