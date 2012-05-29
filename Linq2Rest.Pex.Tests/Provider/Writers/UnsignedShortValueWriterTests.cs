// <copyright file="UnsignedShortValueWriterTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider.Writers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider.Writers
{
    /// <summary>This class contains parameterized unit tests for UnsignedShortValueWriter</summary>
    [PexClass(typeof(UnsignedShortValueWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class UnsignedShortValueWriterTests
    {
        /// <summary>Test stub for get_Handles()</summary>
        [PexMethod]
        internal Type HandlesGet([PexAssumeUnderTest]UnsignedShortValueWriter target)
        {
            Type result = target.Handles;
            return result;
            // TODO: add assertions to method UnsignedShortValueWriterTests.HandlesGet(UnsignedShortValueWriter)
        }
    }
}
