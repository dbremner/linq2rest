// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal static class ExpressionTokenizer
	{
		private static readonly Regex CleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);

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
					openGroups += 1;
				}

				if (openGroups > 0 && blocks[i].EndsWith(")"))
				{
					openGroups -= 1;
				}

				if (openGroups == 0)
				{
					int i1 = i;
					if (blocks[i1].IsOperation())
					{
						int i1 = i;
						var expression1 = startExpression;
						Func<string, int, bool> predicate = (x, j) => j >= expression1 && j < i1;

						if (string.IsNullOrWhiteSpace(currentTokens.Left))
						{
							var expression1 = startExpression;
							int i1 = i;
							currentTokens.Left = string.Join(" ", blocks.Where((x, j) => j >= expression1 && j < i1));
							currentTokens.Operation = blocks[i];
							startExpression = i + 1;

							if (blocks[i1].IsCombinationOperation())
							{
								currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j > i1));

								yield return currentTokens;
								yield break;
							}
						}
						else
						{
							var expression1 = startExpression;
							int i1 = i;
							currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j >= expression1 && j < i1));

							yield return currentTokens;

							startExpression = i1 + 1;
							currentTokens = new TokenSet();

							if (blocks[i1].IsCombinationOperation())
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
			var lastOpen = expression.LastIndexOf('(');
			var lastClose = expression.LastIndexOf(')');

			return lastOpen > lastClose;
		}
	}
}
