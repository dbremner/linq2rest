// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	using Linq2Rest.Provider;

	using Moq;

	using NUnit.Framework;

	public class RestContextTests
	{
		private RestContext<SimpleDto> _provider;
		private Mock<IRestClient> _mockClient;

		[SetUp]
		public void TestSetup()
		{
			var baseUri = new Uri("http://localhost");
			var serializerFactory = new TestSerializerFactory();

			_mockClient = new Mock<IRestClient>();
			_mockClient.SetupGet(x => x.ServiceBase).Returns(baseUri);
			_mockClient.Setup(x => x.Get(It.IsAny<Uri>())).Returns("[{Value : 2, Content : \"blah\" }]");

			_provider = new RestContext<SimpleDto>(_mockClient.Object, serializerFactory);
		}

		[Test]
		public void WhenMainExpressionIsContainedInIsTrueExpressionThenUsesOperandExpression()
		{
			var parameter = Expression.Parameter(typeof(SimpleDto), "x");
			var trueExpression =
				Expression.IsTrue(
				Expression.LessThanOrEqual(Expression.Property(parameter, "Value"), Expression.Constant(3d)));

			var result =
				_provider
				.Query
				.Where(Expression.Lambda<Func<SimpleDto, bool>>(trueExpression, parameter))
				.Count(x => x.ID != 0);

			var uri = new Uri("http://localhost/?$filter=Value+le+3");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryThenCallsRestServiceOnce()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Count(x => x.ID != 0);

			_mockClient.Verify(x => x.Get(It.IsAny<Uri>()), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithFilterThenCallsRestServiceWithFilterParameter()
		{
			var result = _provider.Query
				.Where(x => x.Value <= 3)
				.Count();

			var uri = new Uri("http://localhost/?$filter=Value+le+3");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
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

			var uri = new Uri("http://localhost/?$select=Value");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithMultipleSelectionsThenCallsRestServiceWithSelectParameter()
		{
			var result = _provider.Query
				.Select(x => new { x.Value, x.Content })
				.Count();

			var uri = new Uri("http://localhost/?$select=Value,Content");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithSkipThenCallsRestServiceWithSkipParameter()
		{
			var result = _provider.Query
				.Skip(1)
				.Count();

			var uri = new Uri("http://localhost/?$skip=1");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithTakeThenCallsRestServiceWithTakeParameter()
		{
			var result = _provider.Query
				.Take(1)
				.Count();

			var uri = new Uri("http://localhost/?$top=1");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingQueryWithOrderingThenCallsRestServiceWithOrderParameter()
		{
			var result = _provider.Query
				.OrderBy(x => x.Value)
				.Count();

			var uri = new Uri("http://localhost/?$orderby=Value");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
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

			var uri = new Uri("http://localhost/?$filter=Value+le+3&$select=Value,Content&$skip=1&$top=1&$orderby=Value");
			_mockClient.Verify(x => x.Get(uri), Times.Once());
		}

		[Test]
		public void WhenApplyingNotExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => !(x.Value <= 3), "http://localhost/?$filter=not(Value+le+3)");
		}

		[Test]
		public void WhenApplyingIndexOfExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.IndexOf("text") > -1, "http://localhost/?$filter=indexof(Content,+'text')+gt+-1");
		}

		[Test]
		public void WhenApplyingStartsWithExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.StartsWith("text"), "http://localhost/?$filter=startswith(Content,+'text')");
		}

		[Test]
		public void WhenApplyingEndsWithExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.EndsWith("text"), "http://localhost/?$filter=endswith(Content,+'text')");
		}

		[Test]
		public void WhenApplyingLengthExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.Length > 32, "http://localhost/?$filter=length(Content)+gt+32");
		}

		[Test]
		public void WhenApplyingToLowerExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.ToLower() == "text", "http://localhost/?$filter=tolower(Content)+eq+'text'");
		}

		[Test]
		public void WhenApplyingToLowerInvariantExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.ToLowerInvariant() == "text", "http://localhost/?$filter=tolower(Content)+eq+'text'");
		}

		[Test]
		public void WhenApplyingToUpperExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.ToUpper() == "text", "http://localhost/?$filter=toupper(Content)+eq+'text'");
		}

		[Test]
		public void WhenApplyingToUpperInvariantExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.ToUpperInvariant() == "text", "http://localhost/?$filter=toupper(Content)+eq+'text'");
		}

		[Test]
		public void WhenApplyingTrimExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Content.Trim() == "text", "http://localhost/?$filter=trim(Content)+eq+'text'");
		}

		[Test]
		public void WhenApplyingSecondExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Second == 10, "http://localhost/?$filter=second(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingMinuteExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Minute == 10, "http://localhost/?$filter=minute(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingHourExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Hour == 10, "http://localhost/?$filter=hour(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingDayExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Day == 10, "http://localhost/?$filter=day(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingMonthExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Month == 10, "http://localhost/?$filter=month(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingYearExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => x.Date.Year == 10, "http://localhost/?$filter=year(Date)+eq+10");
		}

		[Test]
		public void WhenApplyingRoundExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => Math.Round(x.Value) == 10d, "http://localhost/?$filter=round(Value)+eq+10");
		}

		[Test]
		public void WhenApplyingFloorExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => Math.Floor(x.Value) == 10d, "http://localhost/?$filter=floor(Value)+eq+10");
		}

		[Test]
		public void WhenApplyingCeilingExpressionThenCallRestServiceWithFilterParameter()
		{
			VerifyCall(x => Math.Ceiling(x.Value) == 10d, "http://localhost/?$filter=ceiling(Value)+eq+10");
		}

		private void VerifyCall(Expression<Func<SimpleDto, bool>> selection, string expectedUri)
		{
			var result = _provider.Query
				.Where(selection)
				.Count();

			_mockClient.Verify(x => x.Get(It.Is<Uri>(u => u.ToString() == expectedUri)), Times.Once());
		}
	}
}
