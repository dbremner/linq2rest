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
using Linq2Rest.Reactive.Moles;
using Linq2Rest.Implementations;
using System.Reactive.Linq;
using Microsoft.Pex.Framework.Explorable;
using System.Collections.Generic;
using Linq2Rest.Provider;
using Microsoft.Pex.Framework;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace Linq2Rest.Reactive
{
	public partial class RestObservableTTests
	{
[Test]
[PexGeneratedBy(typeof(RestObservableTTests))]
public void Poll26()
{
    SIAsyncRestClientFactory sIAsyncRestClientFactory;
    XmlSerializerFactory xmlSerializerFactory;
    RestObservable<int> restObservable;
    IQbservable<int> iQbservable;
    sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
    Type[] types = new Type[0];
    xmlSerializerFactory = PexInvariant.CreateInstance<XmlSerializerFactory>();
    PexInvariant.SetField<IEnumerable<Type>>
        ((object)xmlSerializerFactory, "_knownTypes", (IEnumerable<Type>)types);
    PexInvariant.CheckInvariant((object)xmlSerializerFactory);
    restObservable =
      new RestObservable<int>((IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                              (ISerializerFactory)xmlSerializerFactory);
    iQbservable = this.Poll<int>(restObservable, default(TimeSpan));
    PexAssert.IsNotNull((object)iQbservable);
    PexAssert.IsNotNull((object)restObservable);
}
[Test]
[PexGeneratedBy(typeof(RestObservableTTests))]
public void Poll2601()
{
    SIAsyncRestClientFactory sIAsyncRestClientFactory;
    XmlDataContractSerializerFactory xmlDataContractSerializerFactory;
    RestObservable<int> restObservable;
    IQbservable<int> iQbservable;
    sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
    Type[] types = new Type[0];
    xmlDataContractSerializerFactory =
      PexInvariant.CreateInstance<XmlDataContractSerializerFactory>();
    PexInvariant.SetField<IEnumerable<Type>>
        ((object)xmlDataContractSerializerFactory, 
         "_knownTypes", (IEnumerable<Type>)types);
    PexInvariant.CheckInvariant((object)xmlDataContractSerializerFactory);
    restObservable =
      new RestObservable<int>((IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                              (ISerializerFactory)xmlDataContractSerializerFactory);
    iQbservable = this.Poll<int>(restObservable, default(TimeSpan));
    PexAssert.IsNotNull((object)iQbservable);
    PexAssert.IsNotNull((object)restObservable);
}
[Test]
[PexGeneratedBy(typeof(RestObservableTTests))]
public void Poll2602()
{
    SIAsyncRestClientFactory sIAsyncRestClientFactory;
    JsonDataContractSerializerFactory jsonDataContractSerializerFactory;
    RestObservable<int> restObservable;
    IQbservable<int> iQbservable;
    sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
    Type[] types = new Type[0];
    jsonDataContractSerializerFactory =
      PexInvariant.CreateInstance<JsonDataContractSerializerFactory>();
    PexInvariant.SetField<IEnumerable<Type>>
        ((object)jsonDataContractSerializerFactory, 
         "_knownTypes", (IEnumerable<Type>)types);
    PexInvariant.CheckInvariant((object)jsonDataContractSerializerFactory);
    restObservable =
      new RestObservable<int>((IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                              (ISerializerFactory)jsonDataContractSerializerFactory);
    iQbservable = this.Poll<int>(restObservable, default(TimeSpan));
    PexAssert.IsNotNull((object)iQbservable);
    PexAssert.IsNotNull((object)restObservable);
}
	}
}
