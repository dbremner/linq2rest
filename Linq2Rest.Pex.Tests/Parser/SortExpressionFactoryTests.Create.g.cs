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
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace Linq2Rest.Parser
{
	public partial class SortExpressionFactoryTests
	{
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create276()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, (string)null);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create219()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, "\t");
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create502()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, "\u0100");
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create869()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, ",");
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create602()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, ",\0");
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(SortExpressionFactoryTests))]
public void Create355()
{
    IEnumerable<SortDescription<int>> iEnumerable;
    SortExpressionFactory s0 = new SortExpressionFactory();
    iEnumerable = this.Create<int>(s0, "\0,");
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)s0);
}
	}
}
