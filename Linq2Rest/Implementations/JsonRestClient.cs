// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Implementations
{
	using System;

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
			: base(uri, "application/json")
		{
		}
	}
}