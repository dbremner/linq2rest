// <copyright file="RestClientBaseTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.IO;
using Linq2Rest.Implementations;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for RestClientBase</summary>
    [PexClass(typeof(RestClientBase))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RestClientBaseTests
    {
        /// <summary>Test stub for Dispose()</summary>
        [PexMethod]
        public void Dispose([PexAssumeUnderTest]RestClientBase target)
        {
            target.Dispose();
            // TODO: add assertions to method RestClientBaseTests.Dispose(RestClientBase)
        }

        /// <summary>Test stub for Get(Uri)</summary>
        [PexMethod]
        public Stream Get([PexAssumeUnderTest]RestClientBase target, Uri uri)
        {
            Stream result = target.Get(uri);
            return result;
            // TODO: add assertions to method RestClientBaseTests.Get(RestClientBase, Uri)
        }
    }
}
