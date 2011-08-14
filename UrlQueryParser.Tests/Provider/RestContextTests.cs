// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Tests.Provider
{
	using System;
	using System.Dynamic;
	using System.Linq;
	using System.Web.Script.Serialization;

	using Moq;

	using NUnit.Framework;
	using UrlQueryParser.Provider;

	public class RestContextTests
	{
		private RestContext<SimpleDto> _provider;
		private Mock<IRestClient> _mockClient;

		[SetUp]
		public void TestSetup()
		{
			var baseUri = new Uri("http://localhost");
			var serializer = new JavaScriptSerializer();

			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(baseUri);
			_mockClient.Setup(x => x.GetResponse(It.IsAny<Uri>())).Returns("[]");

			_provider = new RestContext<SimpleDto>(_mockClient.Object, serializer);
		}

		[Test]
		public void WhenApplyingQueryThenCallsRestServiceOnce()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Count(x => x.ID != 0);

			_mockClient.Verify(x => x.GetResponse(It.IsAny<Uri>()), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithFilterThenCallsRestServiceWithFilterParameter()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Count();

			var uri = new Uri("http://localhost/?$filter=Value%20le%203&$select=&$skip=&$take=&$orderby=");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenSelectQueryProjectsIntoMemberWithDifferentNameThenThrows()
		{
			Assert.Throws<InvalidOperationException>(() => _provider.Query.Select(x => new { Something = x.Value }).Count());
		}

		[Test]
		public void WhenApplyingQueryWithSelectionThenCallsRestServiceWithSelectParameter()
		{
			var result = _provider.Query
				.Select(x => new { x.Value })
				.Count();

			var uri = new Uri("http://localhost/?$filter=&$select=Value&$skip=&$take=&$orderby=");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithMultipleSelectionsThenCallsRestServiceWithSelectParameter()
		{
			var result = _provider.Query
				.Select(x => new { x.Value, x.Content })
				.Count();

			var uri = new Uri("http://localhost/?$filter=&$select=Value,Content&$skip=&$take=&$orderby=");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithSkipThenCallsRestServiceWithSkipParameter()
		{
			var result = _provider.Query
				.Skip(1)
				.Count();

			var uri = new Uri("http://localhost/?$filter=&$select=&$skip=1&$take=&$orderby=");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithTakeThenCallsRestServiceWithTakeParameter()
		{
			var result = _provider.Query
				.Take(1)
				.Count();

			var uri = new Uri("http://localhost/?$filter=&$select=&$skip=&$take=1&$orderby=");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithOrderingThenCallsRestServiceWithOrderParameter()
		{
			var result = _provider.Query
				.OrderBy(x => x.Value)
				.Count();

			var uri = new Uri("http://localhost/?$filter=&$select=&$skip=&$take=&$orderby=Value");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingAllOperationsThenCallsRestServiceWithAllParametersSet()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Select(x => new { x.Value, x.Content })
				.OrderBy(x => x.Value)
				.Skip(1)
				.Take(1)
				.Count();

			var uri = new Uri("http://localhost/?$filter=Value%20le%203&$select=Value,Content&$skip=1&$take=1&$orderby=Value");
			_mockClient.Verify(x => x.GetResponse(uri), Times.Once());
		}

		private class SelectionObject : DynamicObject { }
	}
}
