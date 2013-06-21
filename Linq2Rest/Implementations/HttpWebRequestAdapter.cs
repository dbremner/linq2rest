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

        public static HttpWebRequest CreateHttpWebRequest(Uri uri, HttpMethod method, string requestMimeType, string acceptMimeType)
        {
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
