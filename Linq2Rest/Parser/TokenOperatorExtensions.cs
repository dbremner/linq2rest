// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Linq;

	internal static class TokenOperatorExtensions
	{
		private static readonly string[] Operations = new[] { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not", "add", "sub", "mul", "div", "mod" };
		private static readonly string[] Combiners = new[] { "and", "or", "not" };

		public static bool IsCombinationOperation(this string operation)
		{
			return Combiners.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsOperation(this string operation)
		{
			return Operations.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}
	}
}