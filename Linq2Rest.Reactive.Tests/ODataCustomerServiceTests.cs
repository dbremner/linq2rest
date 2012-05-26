// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Reactive;
	using Linq2Rest.Reactive.Implementations;
	using Linq2Rest.Reactive.Tests.Fakes;
	using NUnit.Framework;

	[TestFixture]
	public class ODataCustomerServiceTests
	{
		private RestObservable<NorthwindCustomer> _customerContext;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			// Tests against the sample OData service.
			_customerContext = new RestObservable<NorthwindCustomer>(
				new AsyncJsonRestClientFactory(new Uri("http://services.odata.org/Northwind/Northwind.svc/Customers")),
				new TestODataSerializerFactory());
		}

		[Test]
		public void WhenRequestingCustomerByNameThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
				.Create()
				.Where(x => x.CompanyName.IndexOf("Alfreds") > -1)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenRequestingGroupedCustomerByNameThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
				.Create()
				.Where(x => x.CompanyName.IndexOf("Alfreds") > -1)
				.GroupBy(x => x.CompanyName)
				.Subscribe(x => waitHandle.Set());

			var result = waitHandle.WaitOne(5000);

			Assert.True(result);
		}

		[Test]
		public void WhenRequestingCustomerByNameEndsWithThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
				.Create()
				.Where(x => x.CompanyName.EndsWith("Futterkiste"))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenRequestingCustomerByNameStartsWithThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
				.Create()
				.Where(x => x.CompanyName.StartsWith("Alfr"))
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenRequestingCustomerByNameLengthThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
				.Create()
				.Where(x => x.CompanyName.Length > 10)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}
	}
}