// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal static class TokenOperatorExtensions
	{
		private static readonly string[] Operations = new[] { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not" };
		private static readonly string[] Combiners = new[] { "and", "or", "not" };
		private static readonly string[] Arithmetic = new[] { "add", "sub", "mul", "div", "mod" };

		private static readonly string[] BooleanFunctions = new[] { "substringof", "endswith", "startswith" };
		private static readonly Regex CleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);

		public static bool IsCombinationOperation(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return Combiners.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsOperation(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return Operations.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsArithmetic(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return Arithmetic.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsImpliedBoolean(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			if (!string.IsNullOrWhiteSpace(expression) && !expression.IsEnclosed() && expression.IsFunction())
			{
				var split = expression.Split(' ');
				return !split.Intersect(Operations).Any()
				&& !split.Intersect(Combiners).Any()
				&& BooleanFunctions.Any(x => split[0].StartsWith(x, StringComparison.OrdinalIgnoreCase));
			}

			return false;
		}

		public static Match EnclosedMatch(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			return CleanRx.Match(expression);
		}

		public static bool IsEnclosed(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			var match = expression.EnclosedMatch();
			return match != null && match.Success;
		}

		private static bool IsFunction(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			var open = expression.IndexOf('(');
			var close = expression.IndexOf(')');

			return open > -1 && close > -1;
		}
	}
}