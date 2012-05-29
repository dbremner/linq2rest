// <copyright file="ParameterValueReaderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using Linq2Rest.Parser.Readers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser.Readers
{
    /// <summary>This class contains parameterized unit tests for ParameterValueReader</summary>
    [PexClass(typeof(ParameterValueReader))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ParameterValueReaderTests
    {
        /// <summary>Test stub for Read(Type, String, IFormatProvider)</summary>
        [PexMethod]
        internal Expression Read(
            Type type,
            string token,
            IFormatProvider formatProvider
        )
        {
            Expression result = ParameterValueReader.Read(type, token, formatProvider);
            return result;
            // TODO: add assertions to method ParameterValueReaderTests.Read(Type, String, IFormatProvider)
        }
    }
}
