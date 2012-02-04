// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System;
	using System.Linq;

	using Linq2Rest.Provider;

	using NUnit.Framework;

	public class ODataCustomerServiceTests
	{
		private RestContext<NorthwindCustomer> _customerContext;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			// Tests against the sample OData service.
			_customerContext = new RestContext<NorthwindCustomer>(
				new JsonRestClient(new Uri("http://services.odata.org/Northwind/Northwind.svc/Customers")),
				new TestODataSerializerFactory());
		}

		[Test]
		public void WhenRequestingCustomerByNameThenLoadsCustomer()
		{
			var results = _customerContext.Query.Where(x => x.CompanyName.IndexOf("Alfreds") > -1).ToArray();

			Assert.Less(0, results.Length);
		}

		[Test]
		public void WhenRequestingCustomerByNameEndsWithThenLoadsCustomer()
		{
			var results = _customerContext.Query.Where(x => x.CompanyName.EndsWith("Futterkiste")).ToArray();

			Assert.Less(0, results.Length);
		}

		[Test]
		public void WhenRequestingCustomerByNameStartsWithThenLoadsCustomer()
		{
			var results = _customerContext.Query.Where(x => x.CompanyName.StartsWith("Alfr")).ToArray();

			Assert.Less(0, results.Length);
		}

		[Test]
		public void WhenRequestingCustomerByNameLengthThenLoadsCustomer()
		{
			var results = _customerContext.Query.Where(x => x.CompanyName.Length > 10).ToArray();

			Assert.Less(0, results.Length);
		}
	}
}