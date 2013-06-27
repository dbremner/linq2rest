using System;
using System.IO;
using System.Text;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;

namespace Linq2Rest.Tests.Implementations
{
    using NUnit.Framework;

    [TestFixture]
    class HttpWebRequestAdapterTests
    {
        [Test]
        public void CanCreateTwoHttpPostRequestsAndWriteToThemWithoutBlowingUp()
        {
            //Couldn't figure out a good way to verify the contents of the request stream. Perhaps there is
            //Probably a way to send the request back to yourself and verify the request contents that way.
            //Not really worth it for such a small amount of testing

            var httpRequestFactory = new HttpRequestFactory();

// ReSharper disable InconsistentNaming
            var uri_1          = new Uri("http://test.com");
            var responseMime_1 = "text/xml";
            var requestMime_1  = "text/json";

            var uri_2          = new Uri("http://test.com");
            var responseMime_2 = "text/xml";
            var requestMime_2  = "text/html";

            IHttpRequest httpRequest_1 = httpRequestFactory.Create(uri_1
                                                                  , HttpMethod.Post
                                                                  , responseMime_1
                                                                  , requestMime_1);

            IHttpRequest httpRequest_2 = httpRequestFactory.Create(uri_2
                                                                  , HttpMethod.Post
                                                                  , responseMime_2
                                                                  , requestMime_2);

            var httpWebRequestAdapter_1 = (HttpWebRequestAdapter)httpRequest_1;

            var httpWebRequestAdapter_2 = (HttpWebRequestAdapter)httpRequest_2;

            var expectedRequest_1 = "{ 'value' : 'expected request' }";
            var expectedRequest_2 = "<html>expected request</html>";

            httpWebRequestAdapter_1.WriteRequestStream(new MemoryStream(Encoding.ASCII.GetBytes(expectedRequest_1)));
            httpWebRequestAdapter_2.WriteRequestStream(new MemoryStream(Encoding.ASCII.GetBytes(expectedRequest_2)));
// ReSharper restore InconsistentNaming
        }
    }
}
