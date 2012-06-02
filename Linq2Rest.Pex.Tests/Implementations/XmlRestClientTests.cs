// <copyright file="XmlRestClientTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Implementations;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for XmlRestClient</summary>
    [PexClass(typeof(XmlRestClient))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class XmlRestClientTests
    {
        /// <summary>Test stub for .ctor(Uri)</summary>
		[PexMethod(MaxConditions = 1000)]
        public XmlRestClient Constructor(Uri uri)
        {
            XmlRestClient target = new XmlRestClient(uri);
            return target;
            // TODO: add assertions to method XmlRestClientTests.Constructor(Uri)
        }
    }
}
