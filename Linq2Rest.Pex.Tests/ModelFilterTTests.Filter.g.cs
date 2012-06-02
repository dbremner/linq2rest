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
using System.Linq.Expressions;
using Linq2Rest.Parser;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Explorable;
using System.Web.UI.WebControls;

namespace Linq2Rest
{
	public partial class ModelFilterTTests
	{
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
[ExpectedException(typeof(ArgumentNullException))]
public void FilterThrowsArgumentNullException894()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)null, 0, 0);
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)null);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter857()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)null, 0, 0);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter85701()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)null, 0, int.MinValue);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter85702()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)null, 1, 0);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter85703()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[0];
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[0];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter174()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[1];
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[1];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter803()
{
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[2];
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[2];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter17401()
{
    SortDescription<int> sortDescription;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[1];
    sortDescriptions[0] = sortDescription;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[1];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter17402()
{
    SortDescription<int> sortDescription;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Descending);
    PexInvariant.CheckInvariant((object)sortDescription);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[1];
    sortDescriptions[0] = sortDescription;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[1];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter85704()
{
    SortDescription<int> sortDescription;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[3];
    sortDescriptions[1] = sortDescription;
    sortDescriptions[2] = sortDescription;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[3];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter85705()
{
    SortDescription<int> sortDescription;
    SortDescription<int> sortDescription1;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription);
    sortDescription1 = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription1, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription1, "_direction", SortDirection.Descending);
    PexInvariant.CheckInvariant((object)sortDescription1);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[3];
    sortDescriptions[1] = sortDescription;
    sortDescriptions[2] = sortDescription1;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[3];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter349()
{
    SortDescription<int> sortDescription;
    SortDescription<int> sortDescription1;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription);
    sortDescription1 = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription1, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription1, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription1);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[4];
    sortDescriptions[1] = sortDescription;
    sortDescriptions[2] = sortDescription1;
    sortDescriptions[3] = sortDescription;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[4];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
[Test]
[PexGeneratedBy(typeof(ModelFilterTTests))]
public void Filter86()
{
    SortDescription<int> sortDescription;
    SortDescription<int> sortDescription1;
    SortDescription<int> sortDescription2;
    ModelFilter<int> modelFilter;
    IEnumerable<object> iEnumerable;
    sortDescription = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription);
    sortDescription1 = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription1, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription1, "_direction", SortDirection.Ascending);
    PexInvariant.CheckInvariant((object)sortDescription1);
    sortDescription2 = PexInvariant.CreateInstance<SortDescription<int>>();
    PexInvariant.SetField<Func<int, object>>((object)sortDescription2, 
                                             "_keySelector", PexChoose.CreateDelegate<Func<int, object>>());
    PexInvariant.SetField<SortDirection>
        ((object)sortDescription2, "_direction", SortDirection.Descending);
    PexInvariant.CheckInvariant((object)sortDescription2);
    SortDescription<int>[] sortDescriptions = new SortDescription<int>[6];
    sortDescriptions[1] = sortDescription;
    sortDescriptions[2] = sortDescription1;
    sortDescriptions[3] = sortDescription;
    sortDescriptions[4] = sortDescription2;
    sortDescriptions[5] = sortDescription2;
    modelFilter = new ModelFilter<int>
                      ((Expression<Func<int, bool>>)null, (Expression<Func<int, object>>)null, 
                       (IEnumerable<SortDescription<int>>)sortDescriptions, 0, 0);
    int[] ints = new int[6];
    iEnumerable = this.Filter<int>(modelFilter, (IEnumerable<int>)ints);
    PexAssert.IsNotNull((object)iEnumerable);
    PexAssert.IsNotNull((object)modelFilter);
}
	}
}