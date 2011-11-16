// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;

	using Linq2Rest.Provider;

	using NUnit.Framework;

	public class RestClientTests
	{
		[Test]
		public void WhenCreatingRestClientThenSetsServiceBase()
		{
			var uri = new Uri("http://localhost");

			var client = new RestClient(uri);

			Assert.AreEqual(uri, client.ServiceBase);
		}
	}
}
