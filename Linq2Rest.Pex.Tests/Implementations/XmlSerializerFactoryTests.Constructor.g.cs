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
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Framework;

namespace Linq2Rest.Implementations
{
	public partial class XmlSerializerFactoryTests
	{
[Test]
[PexGeneratedBy(typeof(XmlSerializerFactoryTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void ConstructorThrowsArgumentNullException975()
{
    XmlSerializerFactory xmlSerializerFactory;
    xmlSerializerFactory = this.Constructor((IEnumerable<Type>)null);
}
[Test]
[PexGeneratedBy(typeof(XmlSerializerFactoryTests))]
public void Constructor879()
{
    XmlSerializerFactory xmlSerializerFactory;
    Type[] types = new Type[0];
    xmlSerializerFactory = this.Constructor((IEnumerable<Type>)types);
    PexAssert.IsNotNull((object)xmlSerializerFactory);
}
	}
}