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
using Linq2Rest.Parser;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Engine.Exceptions;
using Linq2Rest.Parser.Moles;
using Microsoft.Pex.Framework;

namespace Linq2Rest
{
	public partial class RuntimeTypeProviderTests
	{
[Test]
[PexGeneratedBy(typeof(RuntimeTypeProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException672()
{
    try
    {
      RuntimeTypeProvider runtimeTypeProvider;
      runtimeTypeProvider = this.Constructor((IMemberNameResolver)null);
      throw 
        new AssertionException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[Test]
[PexGeneratedBy(typeof(RuntimeTypeProviderTests))]
public void Constructor176()
{
    SIMemberNameResolver sIMemberNameResolver;
    RuntimeTypeProvider runtimeTypeProvider;
    sIMemberNameResolver = new SIMemberNameResolver();
    runtimeTypeProvider =
      this.Constructor((IMemberNameResolver)sIMemberNameResolver);
    PexAssert.IsNotNull((object)runtimeTypeProvider);
}
	}
}
