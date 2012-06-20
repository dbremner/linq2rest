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
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace Linq2Rest.Provider
{
	public partial class QueryableExtensionsTests
	{
[Test]
[PexGeneratedBy(typeof(QueryableExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void Expand01ThrowsArgumentNullException307()
{
    IQueryable<int> iQueryable;
    iQueryable = this.Expand01<int>
                     ((IQueryable<int>)null, (Expression<Func<int, object>>[])null);
}
[Test]
[PexGeneratedBy(typeof(QueryableExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void ExpandThrowsArgumentNullException790()
{
    IQueryable<int> iQueryable;
    iQueryable = this.Expand<int>((IQueryable<int>)null, (string)null);
}
	}
}
