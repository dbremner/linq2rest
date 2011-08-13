namespace UrlQueryParser.Tests
{
	using System.Collections.Specialized;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	using Moq;

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

	public class ModelFilterBinderTests
	{
		[Test]
		public void WhenBindingModelThenUsesParameterParser()
		{
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(x => x.Params).Returns(new NameValueCollection());
			var mockContext = new Mock<HttpContextBase>();
			mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);

			var mockParser = new Mock<IParameterParser>();
			var binder = new ModelFilterBinder<FakeItem>(mockParser.Object);

			binder.BindModel(new ControllerContext{ RequestContext = new RequestContext(mockContext.Object, new RouteData()) }, new ModelBindingContext());

			mockParser.Verify(x => x.Parse<FakeItem>(It.IsAny<NameValueCollection>()), Times.Once());
		}
	}
}
