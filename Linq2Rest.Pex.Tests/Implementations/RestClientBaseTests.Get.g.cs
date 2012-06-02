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
using System.IO;
using Microsoft.Pex.Framework.Explorable;
using NUnit.Framework;

namespace Linq2Rest.Implementations
{
	public partial class RestClientBaseTests
	{
[Test]
[PexGeneratedBy(typeof(RestClientBaseTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void GetThrowsArgumentNullException18()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      RestClientBase restClientBase;
      Stream stream;
      restClientBase = PexInvariant.CreateInstance<RestClientBase>();
      PexInvariant.SetField<string>((object)restClientBase, "_acceptHeader", "");
      PexInvariant.SetField<Uri>
          ((object)restClientBase, "<ServiceBase>k__BackingField", (Uri)null);
      PexInvariant.CheckInvariant((object)restClientBase);
      disposables.Add((IDisposable)restClientBase);
      stream = this.Get(restClientBase, (Uri)null);
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
    }
}
[Test]
[PexGeneratedBy(typeof(RestClientBaseTests))]
[ExpectedException(typeof(ArgumentException))]
public void GetThrowsArgumentException980()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      RestClientBase restClientBase;
      Uri uri;
      Stream stream;
      restClientBase = PexInvariant.CreateInstance<RestClientBase>();
      PexInvariant.SetField<string>
          ((object)restClientBase, "_acceptHeader", " /\\0");
      PexInvariant.SetField<Uri>
          ((object)restClientBase, "<ServiceBase>k__BackingField", (Uri)null);
      PexInvariant.CheckInvariant((object)restClientBase);
      disposables.Add((IDisposable)restClientBase);
      uri = new Uri(" /\\0");
      stream = this.Get(restClientBase, uri);
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
    }
}
[Test]
[PexGeneratedBy(typeof(RestClientBaseTests))]
[ExpectedException(typeof(ArgumentException))]
public void GetThrowsArgumentException649()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      RestClientBase restClientBase;
      Uri uri;
      Stream stream;
      restClientBase = PexInvariant.CreateInstance<RestClientBase>();
      PexInvariant.SetField<string>((object)restClientBase, "_acceptHeader", " a-:");
      PexInvariant.SetField<Uri>
          ((object)restClientBase, "<ServiceBase>k__BackingField", (Uri)null);
      PexInvariant.CheckInvariant((object)restClientBase);
      disposables.Add((IDisposable)restClientBase);
      uri = new Uri(" a-:");
      stream = this.Get(restClientBase, uri);
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
    }
}
[Test]
[PexGeneratedBy(typeof(RestClientBaseTests))]
[ExpectedException(typeof(ArgumentException))]
public void GetThrowsArgumentException574()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      RestClientBase restClientBase;
      Uri uri;
      Stream stream;
      restClientBase = PexInvariant.CreateInstance<RestClientBase>();
      PexInvariant.SetField<string>
          ((object)restClientBase, "_acceptHeader", " /\\\u00a0");
      PexInvariant.SetField<Uri>
          ((object)restClientBase, "<ServiceBase>k__BackingField", (Uri)null);
      PexInvariant.CheckInvariant((object)restClientBase);
      disposables.Add((IDisposable)restClientBase);
      uri = new Uri(" /\\\u00a0");
      stream = this.Get(restClientBase, uri);
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
    }
}
[Test]
[PexGeneratedBy(typeof(RestClientBaseTests))]
[ExpectedException(typeof(ArgumentException))]
public void GetThrowsArgumentException739()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      RestClientBase restClientBase;
      Uri uri;
      Stream stream;
      restClientBase = PexInvariant.CreateInstance<RestClientBase>();
      PexInvariant.SetField<string>
          ((object)restClientBase, "_acceptHeader", " /\\-");
      PexInvariant.SetField<Uri>
          ((object)restClientBase, "<ServiceBase>k__BackingField", (Uri)null);
      PexInvariant.CheckInvariant((object)restClientBase);
      disposables.Add((IDisposable)restClientBase);
      uri = new Uri(" /\\-");
      stream = this.Get(restClientBase, uri);
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
    }
}
	}
}