// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpWebRequestAdapter.cs">
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the public interface for an HTTP request.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using Linq2Rest.Provider;

namespace Linq2Rest.Implementations
{
    /// <summary>
    /// Takes a System.Net.HttpWebRequest and wraps it in an IHttpRequest Implementation
    /// </summary>
    class HttpWebRequestAdapter: IHttpRequest
    {
        private readonly HttpWebRequest _httpWebRequest;

        /// <summary>
        /// Creates a basic HttpWebRequest that can then be built off of depending on what other functionality is needed
        /// </summary>
        /// <param name="uri">The uri to send the request to</param>
        /// <param name="method">The Http Request Method</param>
        /// <param name="requestMimeType">The MIME type of the data we are sending</param>
        /// <param name="acceptMimeType">The MIME we accept in response</param>
        /// <returns>Returns an HttpWebRequest initialized with the given parameters</returns>
        public static HttpWebRequest CreateHttpWebRequest(Uri uri, HttpMethod method, string requestMimeType, string acceptMimeType)
        {
            Contract.Requires(uri != null);
            Contract.Requires(acceptMimeType != null);
            Contract.Requires(method != HttpMethod.None);

            requestMimeType = requestMimeType ?? acceptMimeType;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpWebRequest.Method = method.ToString().ToUpperInvariant();

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                httpWebRequest.ContentType = requestMimeType;
            }

            httpWebRequest.Accept = acceptMimeType;

            return httpWebRequest;
        }

        public HttpWebRequestAdapter(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        public Stream GetRequestStream()
        {
            return _httpWebRequest.GetRequestStream();
        }

        public Stream GetResponseStream()
        {
			var response = _httpWebRequest.GetResponse();
			var stream = response.GetResponseStream();
			return stream;
        }
    }
}
