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
using Microsoft.Pex.Framework.Generated;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;

namespace Linq2Rest.Provider
{
	public partial class QueryableExtensionsTests
	{
[Test]
[PexGeneratedBy(typeof(QueryableExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void ExecuteAsyncThrowsArgumentNullException697()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      Task<IEnumerable<int>> task;
      task = this.ExecuteAsync<int>((IQueryable<int>)null);
      disposables.Add((IDisposable)task);
      disposables.Dispose();
    }
}
	}
}
