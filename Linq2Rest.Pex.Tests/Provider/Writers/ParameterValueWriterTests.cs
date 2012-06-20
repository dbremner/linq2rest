// <copyright file="ParameterValueWriterTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using Linq2Rest.Provider.Writers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Provider.Writers
{
    /// <summary>This class contains parameterized unit tests for ParameterValueWriter</summary>
    [PexClass(typeof(ParameterValueWriter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ParameterValueWriterTests
    {
        /// <summary>Test stub for Write(Object)</summary>
        [PexMethod]
        internal string Write(object value)
        {
            string result = ParameterValueWriter.Write(value);
            return result;
            // TODO: add assertions to method ParameterValueWriterTests.Write(Object)
        }
    }
}
