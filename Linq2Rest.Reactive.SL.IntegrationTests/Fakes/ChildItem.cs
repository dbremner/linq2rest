// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.SL.IntegrationTests.Fakes
{
	using System.Collections.Generic;

	public class ChildItem
	{
		public string Text { get; set; }

		public IList<ChildItem> Descendants { get; set; } 
	}
}