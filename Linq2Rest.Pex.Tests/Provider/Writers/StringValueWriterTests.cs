// <copyright file="StringValueWriterTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider.Writers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider.Writers
{
    /// <summary>This class contains parameterized unit tests for StringValueWriter</summary>
    [PexClass(typeof(StringValueWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class StringValueWriterTests
    {
        /// <summary>Test stub for get_Handles()</summary>
        [PexMethod]
        internal Type HandlesGet([PexAssumeUnderTest]StringValueWriter target)
        {
            Type result = target.Handles;
            return result;
            // TODO: add assertions to method StringValueWriterTests.HandlesGet(StringValueWriter)
        }

        /// <summary>Test stub for Write(Object)</summary>
        [PexMethod]
        internal string Write([PexAssumeUnderTest]StringValueWriter target, object value)
        {
            string result = target.Write(value);
            return result;
            // TODO: add assertions to method StringValueWriterTests.Write(StringValueWriter, Object)
        }
    }
}
