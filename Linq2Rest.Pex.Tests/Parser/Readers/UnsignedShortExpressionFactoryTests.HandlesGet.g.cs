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
using Microsoft.Pex.Framework;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace Linq2Rest.Parser.Readers
{
	public partial class UnsignedShortExpressionFactoryTests
	{
[Test]
[PexGeneratedBy(typeof(UnsignedShortExpressionFactoryTests))]
public void HandlesGet802()
{
    Type type;
    UnsignedShortExpressionFactory s0 = new UnsignedShortExpressionFactory();
    type = this.HandlesGet(s0);
    PexAssert.IsNotNull((object)type);
    PexAssert.IsNotNull((object)s0);
}
	}
}