// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRestClient.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines a REST client implementation for JSON requests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Implementations
{
	using System;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// Defines a REST client implementation for JSON requests.
	/// </summary>
	public class JsonRestClient : RestClientBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonRestClient"/> class.
		/// </summary>
		/// <param name="uri">The base <see cref="Uri"/> for the REST service.</param>
		public JsonRestClient(Uri uri)
			: base(uri, StringConstants.JsonMimeType)
		{
			Contract.Requires<ArgumentNullException>(uri != null);
			Contract.Requires<ArgumentException>(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
		}
	}
}