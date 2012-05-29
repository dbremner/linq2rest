// <copyright file="StreamValueWriterTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider.Writers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider.Writers
{
    /// <summary>This class contains parameterized unit tests for StreamValueWriter</summary>
    [PexClass(typeof(StreamValueWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class StreamValueWriterTests
    {
        /// <summary>Test stub for get_Handles()</summary>
        [PexMethod]
        internal Type HandlesGet([PexAssumeUnderTest]StreamValueWriter target)
        {
            Type result = target.Handles;
            return result;
            // TODO: add assertions to method StreamValueWriterTests.HandlesGet(StreamValueWriter)
        }

        /// <summary>Test stub for Write(Object)</summary>
        [PexMethod]
        internal string Write([PexAssumeUnderTest]StreamValueWriter target, object value)
        {
            string result = target.Write(value);
            return result;
            // TODO: add assertions to method StreamValueWriterTests.Write(StreamValueWriter, Object)
        }
    }
}
