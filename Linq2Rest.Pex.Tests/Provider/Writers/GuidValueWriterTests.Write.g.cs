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
using Microsoft.Pex.Engine.Exceptions;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Provider.Writers
{
	public partial class GuidValueWriterTests
	{
[Test]
[PexGeneratedBy(typeof(GuidValueWriterTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void WriteThrowsContractException704()
{
    try
    {
      string s;
      GuidValueWriter s0 = new GuidValueWriter();
      s = this.Write(s0, (object)null);
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
[PexGeneratedBy(typeof(GuidValueWriterTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void WriteThrowsContractException472()
{
    try
    {
      string s;
      GuidValueWriter s0 = new GuidValueWriter();
      object s1 = new object();
      s = this.Write(s0, s1);
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
[PexGeneratedBy(typeof(GuidValueWriterTests))]
public void Write882()
{
    string s;
    GuidValueWriter s0 = new GuidValueWriter();
    object box = (object)(default(Guid));
    s = this.Write(s0, box);
    PexAssert.AreEqual<string>("guid\'00000000-0000-0000-0000-000000000000\'", s);
    PexAssert.IsNotNull((object)s0);
}
	}
}