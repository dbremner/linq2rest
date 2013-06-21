// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataResponse.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ODataResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Linq2Rest.Tests.Fakes
{
	using System.Runtime.Serialization;

	[DataContract]
	public class ODataResponse<T>
	{
		[DataMember(Name = "value")]
		public List<T> Results { get; set; }
	}
}