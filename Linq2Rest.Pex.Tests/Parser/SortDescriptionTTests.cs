// <copyright file="SortDescriptionTTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Web.UI.WebControls;
using Linq2Rest.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Parser
{
    /// <summary>This class contains parameterized unit tests for SortDescription`1</summary>
    [PexClass(typeof(SortDescription<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class SortDescriptionTTests
    {
        /// <summary>Test stub for .ctor(Func`2&lt;!0,Object&gt;, SortDirection)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public SortDescription<T> Constructor<T>(Func<T, object> keySelector, SortDirection direction)
        {
            SortDescription<T> target = new SortDescription<T>(keySelector, direction);
            return target;
            // TODO: add assertions to method SortDescriptionTTests.Constructor(Func`2<!!0,Object>, SortDirection)
        }

        /// <summary>Test stub for get_Direction()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public SortDirection DirectionGet<T>([PexAssumeUnderTest]SortDescription<T> target)
        {
            SortDirection result = target.Direction;
            return result;
            // TODO: add assertions to method SortDescriptionTTests.DirectionGet(SortDescription`1<!!0>)
        }

        /// <summary>Test stub for get_KeySelector()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public Func<T, object> KeySelectorGet<T>([PexAssumeUnderTest]SortDescription<T> target)
        {
            Func<T, object> result = target.KeySelector;
            return result;
            // TODO: add assertions to method SortDescriptionTTests.KeySelectorGet(SortDescription`1<!!0>)
        }
    }
}
