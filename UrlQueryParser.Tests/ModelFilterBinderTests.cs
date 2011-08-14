namespace UrlQueryParser.Tests
{
	using System.Collections.Specialized;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	using Moq;

	using NUnit.Framework;

	using UrlQueryParser.Mvc;
	using UrlQueryParser.Parser;

	public class ModelFilterBinderTests
	{
		[Test]
		public void WhenBindingModelThenUsesParameterParser()
		{
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(x => x.Params).Returns(new NameValueCollection());
			var mockContext = new Mock<HttpContextBase>();
			mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);

			var mockParser = new Mock<IParameterParser<FakeItem>>();
			var binder = new ModelFilterBinder<FakeItem>(mockParser.Object);

			binder.BindModel(new ControllerContext{ RequestContext = new RequestContext(mockContext.Object, new RouteData()) }, new ModelBindingContext());

			mockParser.Verify(x => x.Parse(It.IsAny<NameValueCollection>()), Times.Once());
		}
	}
}