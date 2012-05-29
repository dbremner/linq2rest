// <copyright file="GeneralExtensionsTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.IO;
using Linq2Rest;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest
{
    /// <summary>This class contains parameterized unit tests for GeneralExtensions</summary>
    [PexClass(typeof(GeneralExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class GeneralExtensionsTests
    {
        /// <summary>Test stub for Capitalize(String)</summary>
        [PexMethod]
        internal string Capitalize(string input)
        {
            string result = GeneralExtensions.Capitalize(input);
            return result;
            // TODO: add assertions to method GeneralExtensionsTests.Capitalize(String)
        }

        /// <summary>Test stub for IsAnonymousType(Type)</summary>
        [PexMethod]
        internal bool IsAnonymousType(Type type)
        {
            bool result = GeneralExtensions.IsAnonymousType(type);
            return result;
            // TODO: add assertions to method GeneralExtensionsTests.IsAnonymousType(Type)
        }

        /// <summary>Test stub for ToStream(String)</summary>
        [PexMethod]
        internal Stream ToStream(string input)
        {
            Stream result = GeneralExtensions.ToStream(input);
            return result;
            // TODO: add assertions to method GeneralExtensionsTests.ToStream(String)
        }
    }
}
