// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataResult.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ODataResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive.WP7Sample.Support
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