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
using System.Linq.Expressions;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Engine.Exceptions;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Parser.Readers
{
	public partial class ByteArrayExpressionFactoryTests
	{
[Test]
[PexGeneratedBy(typeof(ByteArrayExpressionFactoryTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConvertThrowsContractException932()
{
    try
    {
      ConstantExpression constantExpression;
      ByteArrayExpressionFactory s0 = new ByteArrayExpressionFactory();
      constantExpression = this.Convert(s0, (string)null);
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
[PexGeneratedBy(typeof(ByteArrayExpressionFactoryTests))]
public void Convert258()
{
    ConstantExpression constantExpression;
    ByteArrayExpressionFactory s0 = new ByteArrayExpressionFactory();
    constantExpression = this.Convert(s0, "");
    PexAssert.IsNotNull((object)constantExpression);
    PexAssert.AreEqual<ExpressionType>
        (ExpressionType.Constant, constantExpression.NodeType);
    PexAssert.IsNull(constantExpression.Value);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(ByteArrayExpressionFactoryTests))]
public void Convert791()
{
    ConstantExpression constantExpression;
    ByteArrayExpressionFactory s0 = new ByteArrayExpressionFactory();
    constantExpression = this.Convert(s0, "\0");
    PexAssert.IsNotNull((object)constantExpression);
    PexAssert.AreEqual<ExpressionType>
        (ExpressionType.Constant, constantExpression.NodeType);
    PexAssert.IsNull(constantExpression.Value);
    PexAssert.IsNotNull((object)s0);
}
	}
}
