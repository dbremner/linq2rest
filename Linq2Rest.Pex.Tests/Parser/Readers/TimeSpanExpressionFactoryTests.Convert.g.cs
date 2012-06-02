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
	public partial class TimeSpanExpressionFactoryTests
	{
[Test]
[PexGeneratedBy(typeof(TimeSpanExpressionFactoryTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConvertThrowsContractException953()
{
    try
    {
      ConstantExpression constantExpression;
      TimeSpanExpressionFactory s0 = new TimeSpanExpressionFactory();
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
[PexGeneratedBy(typeof(TimeSpanExpressionFactoryTests))]
public void Convert184()
{
    ConstantExpression constantExpression;
    TimeSpanExpressionFactory s0 = new TimeSpanExpressionFactory();
    constantExpression = this.Convert(s0, "");
    PexAssert.IsNotNull((object)constantExpression);
    PexAssert.AreEqual<ExpressionType>
        (ExpressionType.Constant, constantExpression.NodeType);
    PexAssert.IsNotNull(constantExpression.Value);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Days);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Hours);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Milliseconds);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Minutes);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Seconds);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(TimeSpanExpressionFactoryTests))]
public void Convert713()
{
    ConstantExpression constantExpression;
    TimeSpanExpressionFactory s0 = new TimeSpanExpressionFactory();
    constantExpression = this.Convert(s0, "\0");
    PexAssert.IsNotNull((object)constantExpression);
    PexAssert.AreEqual<ExpressionType>
        (ExpressionType.Constant, constantExpression.NodeType);
    PexAssert.IsNotNull(constantExpression.Value);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Days);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Hours);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Milliseconds);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Minutes);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Seconds);
    PexAssert.IsNotNull((object)s0);
}
[Test]
[PexGeneratedBy(typeof(TimeSpanExpressionFactoryTests))]
public void Convert845()
{
    ConstantExpression constantExpression;
    TimeSpanExpressionFactory s0 = new TimeSpanExpressionFactory();
    constantExpression = this.Convert(s0, "\0\0\0\0");
    PexAssert.IsNotNull((object)constantExpression);
    PexAssert.AreEqual<ExpressionType>
        (ExpressionType.Constant, constantExpression.NodeType);
    PexAssert.IsNotNull(constantExpression.Value);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Days);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Hours);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Milliseconds);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Minutes);
    PexAssert.AreEqual<int>(0, ((TimeSpan)(constantExpression.Value)).Seconds);
    PexAssert.IsNotNull((object)s0);
}
	}
}