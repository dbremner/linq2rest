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
using Linq2Rest.Parser.Moles;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Parser
{
	public partial class ParameterParserTTests
	{
[Test]
[PexGeneratedBy(typeof(ParameterParserTTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void Constructor01ThrowsArgumentNullException703()
{
    ParameterParser<int> parameterParser;
    parameterParser = this.Constructor01<int>((IFilterExpressionFactory)null, 
                                              (ISortExpressionFactory)null, (ISelectExpressionFactory<int>)null);
}
[Test]
[PexGeneratedBy(typeof(ParameterParserTTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void Constructor01ThrowsArgumentNullException714()
{
    SIFilterExpressionFactory sIFilterExpressionFactory;
    ParameterParser<int> parameterParser;
    sIFilterExpressionFactory = new SIFilterExpressionFactory();
    parameterParser =
      this.Constructor01<int>((IFilterExpressionFactory)sIFilterExpressionFactory, 
                              (ISortExpressionFactory)null, (ISelectExpressionFactory<int>)null);
}
[Test]
[PexGeneratedBy(typeof(ParameterParserTTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void Constructor01ThrowsArgumentNullException440()
{
    SIFilterExpressionFactory sIFilterExpressionFactory;
    SISortExpressionFactory sISortExpressionFactory;
    ParameterParser<int> parameterParser;
    sIFilterExpressionFactory = new SIFilterExpressionFactory();
    sISortExpressionFactory = new SISortExpressionFactory();
    parameterParser =
      this.Constructor01<int>((IFilterExpressionFactory)sIFilterExpressionFactory, 
                              (ISortExpressionFactory)sISortExpressionFactory, 
                              (ISelectExpressionFactory<int>)null);
}
[Test]
[PexGeneratedBy(typeof(ParameterParserTTests))]
public void Constructor01634()
{
    SIFilterExpressionFactory sIFilterExpressionFactory;
    SISortExpressionFactory sISortExpressionFactory;
    SISelectExpressionFactory<int> sISelectExpressionFactory;
    ParameterParser<int> parameterParser;
    sIFilterExpressionFactory = new SIFilterExpressionFactory();
    sISortExpressionFactory = new SISortExpressionFactory();
    sISelectExpressionFactory = new SISelectExpressionFactory<int>();
    parameterParser =
      this.Constructor01<int>((IFilterExpressionFactory)sIFilterExpressionFactory, 
                              (ISortExpressionFactory)sISortExpressionFactory, 
                              (ISelectExpressionFactory<int>)sISelectExpressionFactory);
    PexAssert.IsNotNull((object)parameterParser);
}
[Test]
[PexGeneratedBy(typeof(ParameterParserTTests))]
public void Constructor545()
{
    ParameterParser<int> parameterParser;
    parameterParser = this.Constructor<int>();
    PexAssert.IsNotNull((object)parameterParser);
}
	}
}