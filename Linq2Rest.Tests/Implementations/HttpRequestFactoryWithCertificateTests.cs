namespace Linq2Rest.Tests.Implementations
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Linq2Rest.Implementations;
    using Linq2Rest.Provider;
    using NUnit.Framework;

    [TestFixture]
    class HttpRequestFactoryWithCertificateTests
    {
        [Test]
        public void CanCreateHttpGetRequest()
        {
            var expectedUri = new Uri("http://test.com");
            var expectedMethod = "GET";
            var expectedAccept = "text/html";
            var expectedContentType = null as string;
            var expectedCertificate = new X509Certificate();

            var httpRequestFactory = new HttpRequestFactoryWithCertificate(expectedCertificate);

            IHttpRequest httpRequest  = httpRequestFactory.Create(expectedUri, HttpMethod.Get, "text/html", "text/html");
            var httpWebRequestAdapter = (HttpWebRequestAdapter)httpRequest;
            var actualHttpWebRequest  = httpWebRequestAdapter.HttpWebRequest;

            Assert.AreEqual(expectedUri, actualHttpWebRequest.RequestUri);
            Assert.AreEqual(expectedMethod, actualHttpWebRequest.Method);
            Assert.AreEqual(expectedAccept, actualHttpWebRequest.Accept);
            Assert.AreEqual(expectedContentType, actualHttpWebRequest.ContentType);
            Assert.IsTrue(actualHttpWebRequest.ClientCertificates.Contains(expectedCertificate));
        }

        [Test]
        public void CanCreateHttpPostRequest()
        {
            var expectedUri = new Uri("http://test.com");
            var expectedMethod = "POST";
            var expectedAccept = "text/xml";
            var expectedContentType = "text/json";
            var expectedCertificate = new X509Certificate();

            var httpRequestFactory = new HttpRequestFactoryWithCertificate(expectedCertificate);

            IHttpRequest httpRequest = httpRequestFactory.Create(expectedUri, HttpMethod.Post, "text/xml", "text/json");
            var httpWebRequestAdapter = (HttpWebRequestAdapter)httpRequest;
            var actualHttpWebRequest = httpWebRequestAdapter.HttpWebRequest;

            Assert.AreEqual(expectedUri, actualHttpWebRequest.RequestUri);
            Assert.AreEqual(expectedMethod, actualHttpWebRequest.Method);
            Assert.AreEqual(expectedAccept, actualHttpWebRequest.Accept);
            Assert.AreEqual(expectedContentType, actualHttpWebRequest.ContentType);
            Assert.IsTrue(actualHttpWebRequest.ClientCertificates.Contains(expectedCertificate));
        }

        [Test]
        public void CanCreateHttpPostAndGetRequest()
        {
// ReSharper disable InconsistentNaming

            //Request 1
            var expectedUri_1 = new Uri("http://test.com");
            var expectedMethod_1 = "POST";
            var expectedResponseMime_1 = "text/xml";
            var expectedRequestMime_1 = "text/json";

            //Request 2
            var expectedUri_2 = new Uri("http://test.com");
            var expectedMethod_2 = "GET";
            var expectedResponseMime_2 = "text/html";
            var expectedRequestMime_2 = null as string;

            var expectedCertificate = new X509Certificate();

            var httpRequestFactory = new HttpRequestFactoryWithCertificate(expectedCertificate);


            IHttpRequest httpRequest_1 = httpRequestFactory.Create(expectedUri_1
                                                                  , HttpMethod.Post
                                                                  , expectedResponseMime_1
                                                                  , expectedRequestMime_1);

            IHttpRequest httpRequest_2 = httpRequestFactory.Create(expectedUri_2
                                                                  , HttpMethod.Get
                                                                  , expectedResponseMime_2
                                                                  , expectedRequestMime_2);

            var httpWebRequestAdapter_1 = (HttpWebRequestAdapter)httpRequest_1;
            var actualHttpWebRequest_1 = httpWebRequestAdapter_1.HttpWebRequest;

            var httpWebRequestAdapter_2 = (HttpWebRequestAdapter)httpRequest_2;
            var actualHttpWebRequest_2 = httpWebRequestAdapter_2.HttpWebRequest;

// ReSharper restore InconsistentNaming

            //Request 1
            Assert.AreEqual(expectedUri_1, actualHttpWebRequest_1.RequestUri);
            Assert.AreEqual(expectedMethod_1, actualHttpWebRequest_1.Method);
            Assert.AreEqual(expectedResponseMime_1, actualHttpWebRequest_1.Accept);
            Assert.AreEqual(expectedRequestMime_1, actualHttpWebRequest_1.ContentType);
            Assert.IsTrue(actualHttpWebRequest_1.ClientCertificates.Contains(expectedCertificate));

            //Request 2
            Assert.AreEqual(expectedUri_2, actualHttpWebRequest_2.RequestUri);
            Assert.AreEqual(expectedMethod_2, actualHttpWebRequest_2.Method);
            Assert.AreEqual(expectedResponseMime_2, actualHttpWebRequest_2.Accept);
            Assert.AreEqual(expectedRequestMime_2, actualHttpWebRequest_2.ContentType);
            Assert.IsTrue(actualHttpWebRequest_2.ClientCertificates.Contains(expectedCertificate));

        }
    }
}
