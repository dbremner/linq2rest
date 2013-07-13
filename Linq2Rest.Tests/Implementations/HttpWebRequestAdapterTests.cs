// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpWebRequestAdapterTests.cs" company="INTEGRIS Health" developer="Mark Rucker">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the HttpWebRequestAdaperTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Linq2Rest.Tests.Implementations
{
	using System;
	using System.Net;
	using Linq2Rest.Implementations;
	using NUnit.Framework;

	[TestFixture]
	class HttpWebRequestAdapterTests
	{
		[Test]
		public void HttpWebRequestAdapterShouldReturnUnderlyingRequestStream()
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://test.com"));

			httpWebRequest.Method = "POST";

			var httpWebRequestAdapter = new HttpWebRequestAdapter(httpWebRequest);

			Assert.AreEqual(httpWebRequest.GetRequestStream(), httpWebRequestAdapter.GetRequestStream());
		}

		[Test]
		public void HttpWebRequestAdapterShouldReturnUnderlyingResponseStream()
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://test.com"));
			var httpWebRequestAdapter = new HttpWebRequestAdapter(httpWebRequest);

			Assert.AreEqual(httpWebRequest.GetResponse().GetResponseStream(), httpWebRequestAdapter.GetResponseStream());
		}
	}
}
