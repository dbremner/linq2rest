// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestFactory.cs">
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
using Linq2Rest.Provider;

namespace Linq2Rest.Implementations
{
    /// <summary>
    /// Creates a basic IHttpRequest
    /// </summary>
    class HttpRequestFactory: IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri, HttpMethod method, string acceptMimeType, string requestMimeType)
        {
            var httpWebRequest = HttpWebRequestAdapter.CreateHttpWebRequest(uri, method, acceptMimeType, requestMimeType);

            return new HttpWebRequestAdapter(httpWebRequest);
        }
    }
}
