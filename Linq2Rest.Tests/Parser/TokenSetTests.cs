// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Parser
{
	using Linq2Rest.Parser;

	using NUnit.Framework;

	public class TokenSetTests
	{
		[Test]
		public void TokenSetToStringWritesLeftOperationRight()
		{
			var set = new TokenSet { Left = "Left", Operation = "Operation", Right = "Right" };

			Assert.AreEqual("Left Operation Right", set.ToString());
		}

		[Test]
		public void FunctionTokenSetToStringWritesOperationLeftRight()
		{
			var set = new FunctionTokenSet { Left = "Left", Operation = "Operation", Right = "Right" };

			Assert.AreEqual("Operation Left Right", set.ToString());
		}
	}
}
