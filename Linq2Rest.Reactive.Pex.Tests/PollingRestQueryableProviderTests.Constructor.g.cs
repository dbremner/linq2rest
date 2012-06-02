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
using Linq2Rest.Provider;
using System.Reactive.Concurrency;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Engine.Exceptions;
using Linq2Rest.Reactive.Moles;
using Linq2Rest.Implementations;
using Microsoft.Pex.Framework.Explorable;
using System.Collections.Generic;

namespace Linq2Rest.Reactive
{
	public partial class PollingRestQueryableProviderTests
	{
[Test]
[PexGeneratedBy(typeof(PollingRestQueryableProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException603()
{
    try
    {
      PollingRestQueryableProvider pollingRestQueryableProvider;
      pollingRestQueryableProvider =
        this.Constructor(default(TimeSpan), (IAsyncRestClientFactory)null, 
                         (ISerializerFactory)null, (IScheduler)null, (IScheduler)null);
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
[PexGeneratedBy(typeof(PollingRestQueryableProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException745()
{
    try
    {
      SIAsyncRestClientFactory sIAsyncRestClientFactory;
      PollingRestQueryableProvider pollingRestQueryableProvider;
      sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
      pollingRestQueryableProvider = this.Constructor
                                         (default(TimeSpan), (IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                                          (ISerializerFactory)null, (IScheduler)null, (IScheduler)null);
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
[PexGeneratedBy(typeof(PollingRestQueryableProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException396()
{
    try
    {
      SIAsyncRestClientFactory sIAsyncRestClientFactory;
      XmlSerializerFactory xmlSerializerFactory;
      PollingRestQueryableProvider pollingRestQueryableProvider;
      sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
      Type[] types = new Type[0];
      xmlSerializerFactory = PexInvariant.CreateInstance<XmlSerializerFactory>();
      PexInvariant.SetField<IEnumerable<Type>>
          ((object)xmlSerializerFactory, "_knownTypes", (IEnumerable<Type>)types);
      PexInvariant.CheckInvariant((object)xmlSerializerFactory);
      pollingRestQueryableProvider = this.Constructor
                                         (default(TimeSpan), (IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                                          (ISerializerFactory)xmlSerializerFactory, 
                                          (IScheduler)null, (IScheduler)null);
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
[PexGeneratedBy(typeof(PollingRestQueryableProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException652()
{
    try
    {
      SIAsyncRestClientFactory sIAsyncRestClientFactory;
      JsonDataContractSerializerFactory jsonDataContractSerializerFactory;
      PollingRestQueryableProvider pollingRestQueryableProvider;
      sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
      Type[] types = new Type[0];
      jsonDataContractSerializerFactory =
        PexInvariant.CreateInstance<JsonDataContractSerializerFactory>();
      PexInvariant.SetField<IEnumerable<Type>>
          ((object)jsonDataContractSerializerFactory, 
           "_knownTypes", (IEnumerable<Type>)types);
      PexInvariant.CheckInvariant((object)jsonDataContractSerializerFactory);
      pollingRestQueryableProvider = this.Constructor
                                         (default(TimeSpan), (IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                                          (ISerializerFactory)jsonDataContractSerializerFactory, 
                                          (IScheduler)null, (IScheduler)null);
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
[PexGeneratedBy(typeof(PollingRestQueryableProviderTests))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException617()
{
    try
    {
      SIAsyncRestClientFactory sIAsyncRestClientFactory;
      XmlDataContractSerializerFactory xmlDataContractSerializerFactory;
      PollingRestQueryableProvider pollingRestQueryableProvider;
      sIAsyncRestClientFactory = new SIAsyncRestClientFactory();
      Type[] types = new Type[0];
      xmlDataContractSerializerFactory =
        PexInvariant.CreateInstance<XmlDataContractSerializerFactory>();
      PexInvariant.SetField<IEnumerable<Type>>
          ((object)xmlDataContractSerializerFactory, 
           "_knownTypes", (IEnumerable<Type>)types);
      PexInvariant.CheckInvariant((object)xmlDataContractSerializerFactory);
      pollingRestQueryableProvider = this.Constructor
                                         (default(TimeSpan), (IAsyncRestClientFactory)sIAsyncRestClientFactory, 
                                          (ISerializerFactory)xmlDataContractSerializerFactory, 
                                          (IScheduler)null, (IScheduler)null);
      throw 
        new AssertionException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
	}
}
