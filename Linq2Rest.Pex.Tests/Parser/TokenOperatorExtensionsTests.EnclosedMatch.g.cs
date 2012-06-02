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
using System.Text.RegularExpressions;
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
public void EnclosedMatchThrowsArgumentNullException591()
{
    Match match;
    match = this.EnclosedMatch((string)null);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void EnclosedMatch998()
{
    Match match;
    match = this.EnclosedMatch("");
    PexAssert.IsNotNull((object)match);
    PexAssert.AreEqual<bool>(false, ((Group)match).Success);
    PexAssert.AreEqual<int>(0, ((Capture)match).Index);
    PexAssert.AreEqual<int>(0, ((Capture)match).Length);
}
	}
}
