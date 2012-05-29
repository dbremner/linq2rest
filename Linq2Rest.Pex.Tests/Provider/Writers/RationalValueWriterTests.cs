// <copyright file="RationalValueWriterTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider.Writers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider.Writers
{
    /// <summary>This class contains parameterized unit tests for RationalValueWriter</summary>
    [PexClass(typeof(RationalValueWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RationalValueWriterTests
    {
        /// <summary>Test stub for get_Handles()</summary>
        [PexMethod]
        internal Type HandlesGet([PexAssumeNotNull]RationalValueWriter target)
        {
            Type result = target.Handles;
            return result;
            // TODO: add assertions to method RationalValueWriterTests.HandlesGet(RationalValueWriter)
        }

        /// <summary>Test stub for Write(Object)</summary>
        [PexMethod]
        internal string Write([PexAssumeNotNull]RationalValueWriter target, object value)
        {
            string result = target.Write(value);
            return result;
            // TODO: add assertions to method RationalValueWriterTests.Write(RationalValueWriter, Object)
        }
    }
}
