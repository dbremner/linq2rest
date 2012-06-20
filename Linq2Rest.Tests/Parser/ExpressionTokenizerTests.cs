﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using System.Linq;
	using Linq2Rest.Parser;
	using NUnit.Framework;

	[TestFixture]
	public class ExpressionTokenizerTests
	{
		[Test]
		public void WhenParsingStringWithOneExpressionThenCreatesOneToken()
		{
			const string Expression = "Value eq 1";

			var tokens = Expression.GetTokens();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithSubGroupThenCreatesOneToken()
		{
			const string Expression = "(Name eq 'test') eq true";

			var tokens = Expression.GetTokens();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithSingleQuotesThenCreatesOneToken()
		{
			const string Expression = "Name eq ''single quote' test'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithSingleQuotesAndWithSubGroupThenCreatesOneToken()
		{
			const string Expression = "(Name eq ''single quote' test') eq true";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithDoubleQuotesThenCreatesOneToken()
		{
			const string Expression = "Name eq '\"double quote\" test'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithDoubleQuotesAndWithSubGroupThenCreatesOneToken()
		{
			const string Expression = "(Name eq '\"double quote\" test') eq true";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Count());
		}

		[Test]
		public void WhenParsingStringWithCombinerThenCreatesSeparateTokenSetForCombiner()
		{
			const string Expression = "Value eq 1 and Name eq 'test'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(3, tokens.Length);
			Assert.AreEqual("and", tokens[1].Operation);
			Assert.True(string.IsNullOrWhiteSpace(tokens[1].Left));
			Assert.True(string.IsNullOrWhiteSpace(tokens[1].Right));
		}

		[Test]
		public void WhenParsingStringWithStartingSubGroupAndCombinerThenCreatesOneTokenSet()
		{
			const string Expression = "(Value eq 1 or Number gt 2) and Name eq 'test'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Length);
			Assert.AreEqual("and", tokens[0].Operation);
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Left));
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Right));
		}

		[Test]
		public void WhenParsingStringExtraSpacesArePreserved()
		{
			const string Expression = "Value eq 'extra  spaces'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Length);
			Assert.AreEqual("eq", tokens[0].Operation);
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Left));
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Right));
			Assert.AreEqual("'extra  spaces'", tokens[0].Right);
		}

		[Test]
		public void WhenParsingIgnoreKeywordsInString()
		{
			const string Expression = "Value eq 'text not text'";

			var tokens = Expression.GetTokens().ToArray();

			Assert.AreEqual(1, tokens.Length);
			Assert.AreEqual("eq", tokens[0].Operation);
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Left));
			Assert.True(!string.IsNullOrWhiteSpace(tokens[0].Right));
			Assert.AreEqual("'text not text'", tokens[0].Right);
		}
	}
}
