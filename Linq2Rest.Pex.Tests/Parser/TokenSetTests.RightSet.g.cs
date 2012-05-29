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
using Microsoft.Pex.Framework.Explorable;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Parser
{
	public partial class TokenSetTests
	{
[Test]
[PexGeneratedBy(typeof(TokenSetTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void RightSetThrowsArgumentNullException116()
{
    TokenSet tokenSet;
    tokenSet = PexInvariant.CreateInstance<TokenSet>();
    PexInvariant.SetField<string>((object)tokenSet, "_left", "");
    PexInvariant.SetField<string>((object)tokenSet, "_right", "");
    PexInvariant.SetField<string>((object)tokenSet, "_operation", "");
    PexInvariant.CheckInvariant((object)tokenSet);
    this.RightSet(tokenSet, (string)null);
}
[Test]
[PexGeneratedBy(typeof(TokenSetTests))]
public void RightSet264()
{
    TokenSet tokenSet;
    tokenSet = PexInvariant.CreateInstance<TokenSet>();
    PexInvariant.SetField<string>((object)tokenSet, "_left", "");
    PexInvariant.SetField<string>((object)tokenSet, "_right", "");
    PexInvariant.SetField<string>((object)tokenSet, "_operation", "");
    PexInvariant.CheckInvariant((object)tokenSet);
    this.RightSet(tokenSet, "");
    PexAssert.IsNotNull((object)tokenSet);
}
	}
}
