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
	public partial class DateTimeOffsetValueWriterTests
	{
[Test]
[PexGeneratedBy(typeof(DateTimeOffsetValueWriterTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void WriteThrowsContractException496()
{
    try
    {
      string s;
      DateTimeOffsetValueWriter s0 = new DateTimeOffsetValueWriter();
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
[PexGeneratedBy(typeof(DateTimeOffsetValueWriterTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void WriteThrowsContractException822()
{
    try
    {
      string s;
      DateTimeOffsetValueWriter s0 = new DateTimeOffsetValueWriter();
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
[PexGeneratedBy(typeof(DateTimeOffsetValueWriterTests))]
public void Write983()
{
    string s;
    DateTimeOffsetValueWriter s0 = new DateTimeOffsetValueWriter();
    object box = (object)(default(DateTimeOffset));
    s = this.Write(s0, box);
    PexAssert.AreEqual<string>("datetimeoffset\'0001-01-01T00:00:00Z\'", s);
    PexAssert.IsNotNull((object)s0);
}
	}
}
