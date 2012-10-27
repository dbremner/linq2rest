// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleDto.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SampleDto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Linq2Rest.Reactive.WP8.Sample
{
	[DataContract]
	public class SampleDto
	{
		[DataMember]
		public string Text { get; set; }
	}
}
