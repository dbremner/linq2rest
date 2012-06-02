// <copyright file="ReflectionHelperTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using System.Reflection;
using Linq2Rest.Reactive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive
{
    /// <summary>This class contains parameterized unit tests for ReflectionHelper</summary>
    [PexClass(typeof(ReflectionHelper))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ReflectionHelperTests
    {
        /// <summary>Test stub for get_CreateMethod()</summary>
        [PexMethod]
        internal MethodInfo CreateMethodGet()
        {
            MethodInfo result = ReflectionHelper.CreateMethod;
            return result;
            // TODO: add assertions to method ReflectionHelperTests.CreateMethodGet()
        }
    }
}
