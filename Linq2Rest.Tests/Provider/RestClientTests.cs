// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Provider
{
	using System;
	using Linq2Rest.Implementations;
	using Linq2Rest.Provider;

	using NUnit.Framework;

	public class RestClientTests
	{
		[Test]
		public void WhenCreatingRestClientThenSetsServiceBase()
		{
			var uri = new Uri("http://localhost");

			var client = new JsonRestClient(uri);

			Assert.AreEqual(uri, client.ServiceBase);
		}
	}
}
