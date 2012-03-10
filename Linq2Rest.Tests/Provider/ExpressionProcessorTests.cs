// -----------------------------------------------------------------------
// <copyright file="ExpressionProcessorTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Linq2Rest.Tests.Provider
{
	using System.Linq.Expressions;
	using Linq2Rest.Provider;
	using Moq;
	using NUnit.Framework;

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
