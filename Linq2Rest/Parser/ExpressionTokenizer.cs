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

	internal static class ExpressionTokenizer
	{
		public static IEnumerable<TokenSet> GetTokens(this string expression)
		{
			var tokens = new List<TokenSet>();
			var cleanMatch = expression.EnclosedMatch();

			if (cleanMatch.Success)
			{
				var match = cleanMatch.Groups[1].Value;
				if (!HasOrphanedOpenParenthesis(match))
				{
					expression = match;
				}
			}

			if (expression.IsImpliedBoolean())
			{
				return tokens;
			}

			var blocks = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var openGroups = 0;
			var startExpression = 0;
			var currentTokens = new TokenSet();

			for (int i = 0; i < blocks.Length; i++)
			{
				if (blocks[i].StartsWith("(") || openGroups > 0)
				{
					openGroups += blocks[i].Where(c => c == '(').Count();
				}

				if (openGroups > 0 && blocks[i].EndsWith(")"))
				{
					openGroups -= blocks[i].Where(c => c == ')').Count();
				}

				if (openGroups == 0)
				{
					int i1 = i;
					if (blocks[i1].IsOperation())
					{
						var expression1 = startExpression;

						if (string.IsNullOrWhiteSpace(currentTokens.Left))
						{
							currentTokens.Left = string.Join(" ", blocks.Where((x, j) => j >= expression1 && j < i1));
							currentTokens.Operation = blocks[i];
							startExpression = i + 1;

							if (blocks[i1].IsCombinationOperation())
							{
								currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j > i1));

								//yield return currentTokens;
								//yield break;
								tokens.Add(currentTokens);
								return tokens;
							}
						}
						else
						{
							currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j >= expression1 && j < i1));

							tokens.Add(currentTokens);
							// yield return currentTokens;

							startExpression = i1 + 1;
							currentTokens = new TokenSet();

							if (blocks[i1].IsCombinationOperation())
							{
								// yield return new TokenSet { Operation = blocks[i].ToLowerInvariant() };
								tokens.Add(new TokenSet { Operation = blocks[i].ToLowerInvariant() });
							}
						}
					}
				}
			}

			var remainingToken = string.Join(" ", blocks.Where((x, j) => j >= startExpression));

			if (!string.IsNullOrWhiteSpace(currentTokens.Left))
			{
				currentTokens.Right = remainingToken;
				//yield return currentTokens;
				tokens.Add(currentTokens);
			}
			else if (remainingToken.IsEnclosed())
			{
				currentTokens.Left = remainingToken;
				//yield return currentTokens;
				tokens.Add(currentTokens);
			}
			//else if (!remainingToken.HasOperation())
			//{
			//    tokens.AddRange(GetTokens(remainingToken + " eq true"));
			//}

			return tokens;
		}

		private static bool HasOrphanedOpenParenthesis(string expression)
		{
			Contract.Requires(expression != null);

			var opens = new List<int>();
			var closes = new List<int>();
			var index = expression.IndexOf('(');
			while (index > -1)
			{
				opens.Add(index);
				index = expression.IndexOf('(', index + 1);
			}

			index = expression.IndexOf(')');
			while (index > -1)
			{
				closes.Add(index);
				index = expression.IndexOf(')', index + 1);
			}

			var pairs = opens.Zip(closes, (o, c) => new Tuple<int, int>(o, c));
			var hasOrphan = opens.Count == closes.Count && pairs.Any(x => x.Item2 < x.Item1);

			return hasOrphan;
		}
	}
}
