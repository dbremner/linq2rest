// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContext.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the XmlContext class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Implementations
{
	using System;
	using System.Diagnostics.Contracts;
	using Provider;

	/// <summary>
    /// Defines the XmlContext class.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the item returned from the service.</typeparam>
    public class XmlContext<T> : RestContext<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlContext{T}"/> class. 
        /// </summary>
        /// <param name="source">The <see cref="Uri"/> of the resource collection.</param>
        /// <param name="knownTypes"><see cref="Type"/> to be known by the serializer.</param>
        public XmlContext(Uri source, params Type[] knownTypes)
            : base(new XmlRestClient(source), new XmlDataContractSerializerFactory(knownTypes))
		{
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(knownTypes != null);
			Contract.Requires<ArgumentException>(source.Scheme == Uri.UriSchemeHttp || source.Scheme == Uri.UriSchemeHttps);
        }
    }
}