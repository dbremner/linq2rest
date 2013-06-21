﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpRequestExtensions.cs">
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the public interface for an HTTP request.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Diagnostics.Contracts;
using System.IO;

namespace Linq2Rest.Provider
{
    /// <summary>
    /// Extensions on the IHttpRequest interface (aka Extension Interface Pattern). 
    /// </summary>
    public static class IHttpRequestExtensions
    {
        /// <summary>
        /// Writes a stream to the request stream of an IHttpRequest implementation
        /// </summary>
        /// <param name="httpRequest">The request we are writing our stream to</param>
        /// <param name="inputStream">The stream we want to write to our request</param>
        public static void WriteRequestStream(this IHttpRequest httpRequest, Stream inputStream)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(inputStream != null);

            Stream requestStream = httpRequest.GetRequestStream();

            inputStream.CopyTo(requestStream);
            requestStream.Flush();
        }
    }
}