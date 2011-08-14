namespace UrlQueryParser.Tests
{
	using NUnit.Framework;

	using UrlQueryParser.Mvc;
	using UrlQueryParser.Parser;

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
