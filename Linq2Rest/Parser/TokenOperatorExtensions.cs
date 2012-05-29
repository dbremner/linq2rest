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
		private static readonly string[] _operations = new[] { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not" };
		private static readonly string[] _combiners = new[] { "and", "or", "not" };
		private static readonly string[] _arithmetic = new[] { "add", "sub", "mul", "div", "mod" };

		private static readonly string[] _booleanFunctions = new[] { "substringof", "endswith", "startswith" };
        private static readonly Regex _collectionFunctionRx = new Regex(@"^[0-9a-zA-Z_]+/(all|any)\((.+)\)$", RegexOptions.Compiled);
		private static readonly Regex _cleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);
		private static readonly Regex _stringStartRx = new Regex("^[(]*'", RegexOptions.Compiled);
		private static readonly Regex _stringEndRx = new Regex("'[)]*$", RegexOptions.Compiled);

		public static bool IsCombinationOperation(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return _combiners.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsOperation(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return _operations.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsArithmetic(this string operation)
		{
			Contract.Requires<ArgumentNullException>(operation != null);

			return _arithmetic.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
		}

		public static bool IsImpliedBoolean(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			if (!string.IsNullOrWhiteSpace(expression) && !expression.IsEnclosed() && expression.IsFunction())
			{
				var split = expression.Split(' ');
				return !split.Intersect(_operations).Any()
				&& !split.Intersect(_combiners).Any()
				&& (_booleanFunctions.Any(x => split[0].StartsWith(x, StringComparison.OrdinalIgnoreCase)) ||
                    _collectionFunctionRx.IsMatch(expression));
			}

			return false;
		}

		public static Match EnclosedMatch(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			return _cleanRx.Match(expression);
		}

		public static bool IsEnclosed(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			var match = expression.EnclosedMatch();
			return match != null && match.Success;
		}

		public static bool IsStringStart(this string expression)
		{
			return !string.IsNullOrWhiteSpace(expression) && _stringStartRx.IsMatch(expression);
		}

		public static bool IsStringEnd(this string expression) 
		{
			return !string.IsNullOrWhiteSpace(expression) && _stringEndRx.IsMatch(expression);
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