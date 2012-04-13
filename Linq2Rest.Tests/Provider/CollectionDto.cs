// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Collections.Generic;

	public class CollectionDto
	{
		public int ID { get; set; }

		public string Content { get; set; }

		public double Value { get; set; }

		public DateTime Date { get; set; }

		public Choice Choice { get; set; }

		public ICollection<ChildDto> Children { get; set; }
	}
}