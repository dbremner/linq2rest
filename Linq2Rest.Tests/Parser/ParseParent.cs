// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	public class ParseParent
	{
		public ParseObject Item { get; set; }

		public int Number { get; set; }

		public class ParseObject
		{
			public int Value { get; set; }

			public static ParseObject Parse(string input)
			{
				var value = int.Parse(input);
				return new ParseObject { Value = value };
			}

			public override string ToString()
			{
				return Value.ToString();
			}
		}
	}
}