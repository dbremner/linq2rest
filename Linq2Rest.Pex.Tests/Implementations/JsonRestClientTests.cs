// <copyright file="JsonRestClientTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Implementations;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
    /// <summary>This class contains parameterized unit tests for JsonRestClient</summary>
    [PexClass(typeof(JsonRestClient))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class JsonRestClientTests
    {
        /// <summary>Test stub for .ctor(Uri)</summary>
		[PexMethod(MaxConditions = 1000)]
        public JsonRestClient Constructor(Uri uri)
        {
            JsonRestClient target = new JsonRestClient(uri);
            return target;
            // TODO: add assertions to method JsonRestClientTests.Constructor(Uri)
        }
    }
}
