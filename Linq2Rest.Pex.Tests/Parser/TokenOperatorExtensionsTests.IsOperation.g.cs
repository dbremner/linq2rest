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
public void IsOperationThrowsArgumentNullException872()
{
    bool b;
    b = this.IsOperation((string)null);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsOperation730()
{
    bool b;
    b = this.IsOperation("");
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsOperation570()
{
    bool b;
    b = this.IsOperation("eq");
    PexAssert.AreEqual<bool>(true, b);
}
	}
}
