// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class ExpressionTokenizer
	{
		private static readonly Regex CleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);
		private static readonly string[] Operations = new[] { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not", "add", "sub", "mul", "div", "mod" };
		private static readonly string[] Combiners = new [] { "and", "or", "not" };

		public IEnumerable<TokenSet> GetTokens(string expression)
		{
			var cleanMatch = CleanRx.Match(expression);

			if (cleanMatch.Success)
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
					if (Operations.Any(x => string.Equals(x, blocks[i], StringComparison.OrdinalIgnoreCase)))
					{
						if (string.IsNullOrWhiteSpace(currentTokens.Left))
						{
							var expression1 = startExpression;
							int i1 = i;
							currentTokens.Left = string.Join(" ", blocks.Where((x, j) => j >= expression1 && j < i1));
							currentTokens.Operation = blocks[i];
							startExpression = i + 1;

							if (Combiners.Any(x => string.Equals(x, blocks[i], StringComparison.OrdinalIgnoreCase)))
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

			if (!string.IsNullOrWhiteSpace(currentTokens.Left))
			{
				currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j >= startExpression));
				yield return currentTokens;
			}
		}
	}
}
