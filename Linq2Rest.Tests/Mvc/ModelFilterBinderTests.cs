// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelFilterBinderTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ModelFilterBinderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Mvc
{
	using System.Collections.Specialized;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Linq2Rest.Mvc;
	using Linq2Rest.Parser;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
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

			binder.BindModel(new ControllerContext { RequestContext = new RequestContext(mockContext.Object, new RouteData()) }, new ModelBindingContext());

			mockParser.Verify(x => x.Parse(It.IsAny<NameValueCollection>()), Times.Once());
		}
	}
}