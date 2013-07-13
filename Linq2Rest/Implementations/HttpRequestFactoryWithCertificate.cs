// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestFactoryWithCertificate.cs" company="INTEGRIS Health" developer="Mark Rucker">
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines a factory that creates IHttpRequest with a certificate attached to them.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Implementations
{
	using System;
	using System.Security.Cryptography.X509Certificates;
	using Provider;

	/// <summary>
    /// Creates an IHttpRequest with the given certificate attached to it
    /// </summary>
    public class HttpRequestFactoryWithCertificate : IHttpRequestFactory
    {
        private readonly X509Certificate _clientCertificate;

        /// <summary>
        /// Creates an HttpRequestFactoryWithCertificate
        /// </summary>
        /// <param name="clientCertificate">The client certificate to pass with the http request</param>
        public HttpRequestFactoryWithCertificate(X509Certificate clientCertificate)
        {
            _clientCertificate = clientCertificate;
        }

        /// <summary>
        /// Creates an IHttpRequest that can be used to send an http request
        /// </summary>
        /// <param name="uri">The location the request is to be sent to</param>
        /// <param name="method">The method to use to send the request</param>
        /// <param name="responseMimeType">The Mime type we accept in response</param>
        /// <param name="requestMimeType">The Mime type we are sending in request</param>
        /// <returns>The HttpRequest we are creating</returns>
        public IHttpRequest Create(Uri uri, HttpMethod method, string responseMimeType, string requestMimeType)
        {
            var httpWebRequest = HttpWebRequestAdapter.CreateHttpWebRequest(uri, method, responseMimeType, requestMimeType);

            httpWebRequest.ClientCertificates.Add(_clientCertificate);

            return new HttpWebRequestAdapter(httpWebRequest);
        }
    }
}
