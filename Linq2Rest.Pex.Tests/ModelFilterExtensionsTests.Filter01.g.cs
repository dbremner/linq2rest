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
using System.Collections.Specialized;

namespace Linq2Rest
{
	public partial class ModelFilterExtensionsTests
	{
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void Filter01ThrowsArgumentNullException932()
{
    IEnumerable<object> iEnumerable;
    iEnumerable =
      this.Filter01<int>((IEnumerable<int>)null, (IModelFilter<int>)null);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter01276()
{
    IEnumerable<object> iEnumerable;
    int[] ints = new int[0];
    iEnumerable =
      this.Filter01<int>((IEnumerable<int>)ints, (IModelFilter<int>)null);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void FilterThrowsArgumentNullException247()
{
    IEnumerable<object> iEnumerable;
    iEnumerable =
      this.Filter<int>((IEnumerable<int>)null, (NameValueCollection)null);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void FilterThrowsArgumentNullException193()
{
    IEnumerable<object> iEnumerable;
    int[] ints = new int[0];
    iEnumerable =
      this.Filter<int>((IEnumerable<int>)ints, (NameValueCollection)null);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter492()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[0];
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter49201()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[1];
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter103()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[1];
    KeyValuePair<string, string> s0 = new KeyValuePair<string, string>("", "");
    keyValuePairs[0] = s0;
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[1];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter10301()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[2];
    KeyValuePair<string, string> s0 = new KeyValuePair<string, string>("", "");
    keyValuePairs[0] = s0;
    KeyValuePair<string, string> s1 = new KeyValuePair<string, string>("", "");
    keyValuePairs[1] = s1;
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[1];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter109()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[3];
    KeyValuePair<string, string> s0
       = new KeyValuePair<string, string>("\ue8e3", "\u3794\u8f43\u8475");
    keyValuePairs[0] = s0;
    KeyValuePair<string, string> s1 = 
                                     new KeyValuePair<string, string>("\u3794\u8f43\u8475", "\u3794\u8f43\u8475");
    keyValuePairs[1] = s1;
    KeyValuePair<string, string> s2 = 
                                     new KeyValuePair<string, string>("\u3794\u8f43\u8475", "\u3794\u8f43\u8475");
    keyValuePairs[2] = s2;
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[3];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterExtensionsTests))]
public void Filter10901()
{
    NameValueCollection nameValueCollection;
    IEnumerable<object> iEnumerable;
    KeyValuePair<string, string>[] keyValuePairs
       = new KeyValuePair<string, string>[3];
    KeyValuePair<string, string> s0
       = new KeyValuePair<string, string>("", "\u34a0\u524a");
    keyValuePairs[0] = s0;
    KeyValuePair<string, string> s1 = 
                                     new KeyValuePair<string, string>("\ud443\ubea7\ubdfe\u440b", "\u34a0\u524a");
    keyValuePairs[1] = s1;
    KeyValuePair<string, string> s2
       = new KeyValuePair<string, string>("\u34a0\u524a", "\u34a0\u524a");
    keyValuePairs[2] = s2;
    nameValueCollection = PexFactories.CreateNameValueCollection(keyValuePairs);
    int[] ints = new int[3];
    iEnumerable = this.Filter<int>((IEnumerable<int>)ints, nameValueCollection);
    PexAssert.IsNotNull((object)iEnumerable);
}
	}
}