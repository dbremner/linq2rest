// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Runtime.Serialization;

	[DataContract]
	public class FakeChildItem
	{
		private readonly Collection<FakeGrandChildItem> _children = new Collection<FakeGrandChildItem>();

		public int ID { get; set; }

		public string ChildStringValue { get; set; }

		public ICollection<FakeGrandChildItem> Children
		{
			get { return _children; }
		}
	}
}