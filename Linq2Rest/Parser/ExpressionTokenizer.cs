// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal static class ExpressionTokenizer
	{
		private static readonly Regex CleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);
		private static readonly string[] Operations = new[] { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not", "add", "sub", "mul", "div", "mod" };
		private static readonly string[] Combiners = new[] { "and", "or", "not" };

		public static IEnumerable<TokenSet> GetTokens(this string expression)
		{
			var cleanMatch = CleanRx.Match(expression);

			if (cleanMatch.Success && !HasOrphanedOpenParenthesis(cleanMatch.Groups[1].Value))
			{
				expression = cleanMatch.Groups[1].Value;
			}

			var blocks = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var openGroups = 0;
			var startExpression = 0;
			var currentTokens = new TokenSet();

			for (int i = 0; i < blocks.Length; i++)
			{
				if (blocks[i].StartsWith("("))
				{
					var currentBlock = blocks[i];
					var openBlocks = currentBlock.TakeWhile(c => c == '(').Count();
					openGroups += openBlocks;
				}

				if (openGroups > 0 && blocks[i].EndsWith(")"))
				{
					var currentBlock = blocks[i];
					var openMethods = currentBlock.Count(c => c == '(');
					var closeBlocks = currentBlock.Reverse().TakeWhile(c => c == ')').Count() - openMethods;

					openGroups -= closeBlocks;
				}

				if (openGroups == 0)
				{
					int i1 = i;
					if (Operations.Any(x => string.Equals(x, blocks[i1], StringComparison.OrdinalIgnoreCase)))
					{
						int i1 = i;
						var expression1 = startExpression;
						Func<string, int, bool> predicate = (x, j) => j >= expression1 && j < i1;

						if (string.IsNullOrWhiteSpace(currentTokens.Left))
						{
							currentTokens.Left = string.Join(" ", blocks.Where(predicate));
							currentTokens.Operation = blocks[i];
							startExpression = i + 1;

							if (Combiners.Any(x => string.Equals(x, blocks[i1], StringComparison.OrdinalIgnoreCase)))
							{
								currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j > i1));

								yield return currentTokens;
								yield break;
							}
						}
						else
						{
							currentTokens.Right = string.Join(" ", blocks.Where(predicate));

							yield return currentTokens;

							startExpression = i + 1;
							currentTokens = new TokenSet();

							if (Combiners.Any(x => string.Equals(x, blocks[i], StringComparison.OrdinalIgnoreCase)))
							{
								yield return new TokenSet { Operation = blocks[i].ToLowerInvariant() };
							}
						}
					}
				}
			}

			var remainingToken = string.Join(" ", blocks.Where((x, j) => j >= startExpression));

			if (!string.IsNullOrWhiteSpace(currentTokens.Left))
			{
				currentTokens.Right = remainingToken;
				yield return currentTokens;
			}
			else if (CleanRx.IsMatch(remainingToken))
			{
				currentTokens.Left = remainingToken;
				yield return currentTokens;
			}
		}

		private static bool HasOrphanedOpenParenthesis(string expression)
		{
			Contract.Requires(expression != null);

			var lastOpen = expression.LastIndexOf('(');
			var lastClose = expression.LastIndexOf(')');

			return lastOpen > lastClose;
		}
	}
}