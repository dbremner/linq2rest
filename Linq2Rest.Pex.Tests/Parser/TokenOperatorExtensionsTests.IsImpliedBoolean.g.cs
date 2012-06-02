// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Parser
{
	public partial class TokenOperatorExtensionsTests
	{
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void IsImpliedBooleanThrowsArgumentNullException687()
{
    bool b;
    b = this.IsImpliedBoolean((string)null);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean680()
{
    bool b;
    b = this.IsImpliedBoolean("");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean606()
{
    bool b;
    b = this.IsImpliedBoolean("\0");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean736()
{
    bool b;
    b = this.IsImpliedBoolean("(");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean119()
{
    bool b;
    b = this.IsImpliedBoolean("(\u0100");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean190()
{
    bool b;
    b = this.IsImpliedBoolean("()");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsImpliedBoolean372()
{
    bool b;
    b = this.IsImpliedBoolean(" \u0100()");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
[Ignore("the test state was: duplicate path")]
public void IsImpliedBoolean947()
{
    bool b;
    b = this.IsImpliedBoolean("(\u0100 )");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
[Ignore("the test state was: duplicate path")]
public void IsImpliedBoolean43()
{
    bool b;
    b = this.IsImpliedBoolean(" \ue4d8()");
    PexAssert.AreEqual<bool>(false, b);
}
	}
}
