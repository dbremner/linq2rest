// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
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
		private static readonly Regex FunctionRx = new Regex(@"^([^\(\)]+)\((.+)\)$", RegexOptions.Compiled);
		private static readonly Regex FunctionContentRx = new Regex(@"^(.*\((?>[^()]+|\((?<Depth>.*)|\)(?<-Depth>.*))*(?(Depth)(?!))\)|.*?)\s*,\s*(.+)$", RegexOptions.Compiled);
		private static readonly Regex AnyAllFunctionRx = new Regex(@"^([0-9a-zA-Z]+/)?(([0-9a-zA-Z_]+/)+)(any|all)\((.*)\)$", RegexOptions.Compiled);

		public static IEnumerable<TokenSet> GetTokens(this string expression)
		{
			Contract.Ensures(Contract.Result<IEnumerable<TokenSet>>() != null);

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
				yield break;
			}

			var blocks = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var openGroups = 0;
			var startExpression = 0;
			var currentTokens = new TokenSet();

			for (int i = 0; i < blocks.Length; i++)
			{
				var netEnclosed = blocks[i].Count(c => c == '(') - blocks[i].Count(c => c == ')');
				openGroups += netEnclosed;

				if (openGroups == 0)
				{
					if (blocks[i].IsOperation())
					{
						var expression1 = startExpression;
						Func<string, int, bool> predicate = (x, j) => j >= expression1 && j < i;

						if (string.IsNullOrWhiteSpace(currentTokens.Left))
						{
							currentTokens.Left = string.Join(" ", blocks.Where(predicate));
							currentTokens.Operation = blocks[i];
							startExpression = i + 1;

							if (blocks[i].IsCombinationOperation())
							{
								currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j > i));

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

							if (blocks[i].IsCombinationOperation())
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
			else if (remainingToken.IsEnclosed())
			{
				currentTokens.Left = remainingToken;
				yield return currentTokens;
			}
		}

		public static TokenSet GetArithmeticToken(this string expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			var cleanMatch = expression.EnclosedMatch();

			if (cleanMatch.Success)
			{
				var match = cleanMatch.Groups[1].Value;
				if (!HasOrphanedOpenParenthesis(match))
				{
					expression = match;
				}
			}

			var blocks = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var hasOperation = blocks.Any(x => x.IsArithmetic());
			if (!hasOperation)
			{
				return null;
			}

			var operationIndex = GetArithmeticOperationIndex(blocks);

			var left = string.Join(" ", blocks.Where((x, i) => i < operationIndex));
			var right = string.Join(" ", blocks.Where((x, i) => i > operationIndex));
			var operation = blocks[operationIndex];

			return new TokenSet { Left = left, Operation = operation, Right = right };
		}

		public static TokenSet GetAnyAllFunctionTokens(this string filter)
		{
			Contract.Requires(filter != null);

			var functionMatch = AnyAllFunctionRx.Match(filter);
			if (!functionMatch.Success)
			{
				return null;
			}

			var functionCollection = functionMatch.Groups[2].Value.Trim('/');
			var functionName = functionMatch.Groups[4].Value;
			var functionContent = functionMatch.Groups[5].Value;

			return new FunctionTokenSet
			{
				Operation = functionName,
				Left = functionCollection,
				Right = functionContent
			};
		}

		public static TokenSet GetFunctionTokens(this string filter)
		{
			Contract.Requires(filter != null);

			var functionMatch = FunctionRx.Match(filter);
			if (!functionMatch.Success)
			{
				return null;
			}

			var functionName = functionMatch.Groups[1].Value;
			var functionContent = functionMatch.Groups[2].Value;
			var functionContentMatch = FunctionContentRx.Match(functionContent);
			if (!functionContentMatch.Success)
			{
				return new FunctionTokenSet
				{
					Operation = functionName,
					Left = functionContent
				};
			}

			return new FunctionTokenSet
			{
				Operation = functionName,
				Left = functionContentMatch.Groups[1].Value,
				Right = functionContentMatch.Groups[2].Value
			};
		}

		private static int GetArithmeticOperationIndex(IList<string> blocks)
		{
			Contract.Requires(blocks != null);

			var openGroups = 0;
			var operationIndex = -1;
			for (var i = 0; i < blocks.Count; i++)
			{
				var source = blocks[i];

				Contract.Assume(source != null, "Does not generate null token parts.");

				var netEnclosed = source.Count(c => c == '(') - source.Count(c => c == ')');
				openGroups += netEnclosed;

				if (openGroups == 0 && source.IsArithmetic())
				{
					operationIndex = i;
				}
			}

			return operationIndex;
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
