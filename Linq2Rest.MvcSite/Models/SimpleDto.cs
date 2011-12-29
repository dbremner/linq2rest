// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.MvcSite.Models
{
	using System.ComponentModel.DataAnnotations;

	public class SimpleDto
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public string Content { get; set; }

		public double Value { get; set; }
	}
}