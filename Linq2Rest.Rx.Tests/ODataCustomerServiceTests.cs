// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System;
	using System.Linq;
	using System.Reactive.Linq;
	using System.Threading;
	using Linq2Rest.Implementations;
	using Linq2Rest.Provider;
	using Linq2Rest.Rx;
	using Linq2Rest.Rx.Implementations;
	using Linq2Rest.Rx.Tests.Fakes;
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
				.Where(x => x.CompanyName.IndexOf("Alfreds") > -1)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}

		[Test]
		public void WhenRequestingCustomerByNameEndsWithThenLoadsCustomer()
		{
			var waitHandle = new ManualResetEvent(false);

			_customerContext
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
				.Where(x => x.CompanyName.Length > 10)
				.Subscribe(x => waitHandle.Set(), () => waitHandle.Set());

			var result = waitHandle.WaitOne(2000);

			Assert.True(result);
		}
	}
}