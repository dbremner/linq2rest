// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Linq;

	using Linq2Rest.Parser;

	using NUnit.Framework;

	public class ExpressionTokenizerTests
	{
		private ExpressionTokenizer _tokenizer;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_tokenizer = new ExpressionTokenizer();
		}

		[Test]
		public void WhenParsingStringWithOneExpressionThenCreatesOneToken()
		{
			const string Expression = "Value eq 1";

			var tokens = _tokenizer.GetTokens(Expression);

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithSubGroupThenCreatesOneToken()
		{
			const string Expression = "(Name eq 'test') eq true";

			var tokens = _tokenizer.GetTokens(Expression);

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithCombinerThenCreatesSeparateTokenSetForCombiner()
		{
			const string Expression = "Value eq 1 and Name eq 'test'";

			var tokens = _tokenizer.GetTokens(Expression).ToArray();

			Assert.AreEqual(3, tokens.Length);
			Assert.AreEqual("and", tokens[1].Operation);
			Assert.True(string.IsNullOrWhiteSpace(tokens[1].Left));
			Assert.True(string.IsNullOrWhiteSpace(tokens[1].Right));
		}

		[Test]
		public void WhenParsingStringWithStartingSubGroupAndCombinerThenCreatesOneTokenSet()
		{
			const string Expression = "(Value eq 1 or Number gt 2) and Name eq 'test'";

			var tokens = _tokenizer.GetTokens(Expression).ToArray();

			Assert.AreEqual(1, tokens.Length);
			Assert.AreEqual("and", tokens[0].Operation);
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Left));
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Right));
		}
	}
}
