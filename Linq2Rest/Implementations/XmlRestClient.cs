// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Implementations
{
	using System;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// Defines a REST client implementation for JSON requests.
	/// </summary>
	public class XmlRestClient : RestClientBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlRestClient"/> class.
		/// </summary>
		/// <param name="uri">The base <see cref="Uri"/> for the REST service.</param>
		public XmlRestClient(Uri uri)
			: base(uri, StringConstants.XmlMimeType)
		{
			Contract.Requires<ArgumentNullException>(uri != null);
			Contract.Requires<ArgumentException>(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
		}
	}
}