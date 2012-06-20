// <copyright file="MemberNameResolverTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Reflection;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for MemberNameResolver</summary>
    [PexClass(typeof(MemberNameResolver))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class MemberNameResolverTests
    {
        /// <summary>Test stub for ResolveName(MemberInfo)</summary>
        [PexMethod]
        internal string ResolveName(
            [PexAssumeUnderTest]MemberNameResolver target,
            MemberInfo member
        )
        {
            string result = target.ResolveName(member);
            return result;
            // TODO: add assertions to method MemberNameResolverTests.ResolveName(MemberNameResolver, MemberInfo)
        }
    }
}
