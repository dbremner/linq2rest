// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
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