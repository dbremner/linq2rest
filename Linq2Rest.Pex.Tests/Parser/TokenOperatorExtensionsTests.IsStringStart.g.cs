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
public void IsStringStart25()
{
    bool b;
    b = this.IsStringStart((string)null);
    PexAssert.AreEqual<bool>(false, b);
}
[Test]
[PexGeneratedBy(typeof(TokenOperatorExtensionsTests))]
public void IsStringStart606()
{
    bool b;
    b = this.IsStringStart("\0");
    PexAssert.AreEqual<bool>(false, b);
}
	}
}
