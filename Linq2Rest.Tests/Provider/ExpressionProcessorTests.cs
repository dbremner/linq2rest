// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest.Tests.Provider
{
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class ExpressionProcessorTests
	{
		[Test]
		public void WhenProcessingExpressionThenCallsExpressionVisitor()
		{
			var mockVisitor = new Mock<IExpressionVisitor>();
			var processor = new ExpressionProcessor(mockVisitor.Object);
			var expression = Expression.Constant(1);

			processor.Process(expression);

			mockVisitor.Verify(x => x.Visit(expression), Times.Once());
		}
	}
}
