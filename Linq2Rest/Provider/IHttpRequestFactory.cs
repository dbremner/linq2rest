// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpRequestFactory.cs">
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

namespace Linq2Rest.Provider
{
    /// <summary>
    /// Defines the public interface for a Lin2Rest.Provider.IHttpRequest object factory
    /// </summary>
    [ContractClass(typeof(HttpRequestFactoryContracts))]
    public interface IHttpRequestFactory
    {
        /// <summary>
        /// Creates an IHttpRequest that can be used to send an httpRequest
        /// </summary>
        /// <param name="uri">The location the request is to be sent to</param>
        /// <param name="method">The method to use to send the request</param>
        /// <param name="acceptMimeType">The Mime type we accept in response</param>
        /// <param name="requestMimeType">The Mime type we are sending in request</param>
        /// <returns>The HttpRequest we are creating</returns>
        IHttpRequest Create(Uri uri, HttpMethod method, string acceptMimeType, string requestMimeType);
    }

    [ContractClassFor(typeof(IHttpRequestFactory))]
    internal class HttpRequestFactoryContracts:IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri, HttpMethod method, string acceptMimeType, string requestMimeType = null)
        {
            Contract.Requires(uri != null);
            Contract.Requires(acceptMimeType != null);
            Contract.Requires(method != HttpMethod.None);

            throw new NotImplementedException();
        }
    }
}
