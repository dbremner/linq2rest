// <copyright file="AsyncXmlRestClientFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using Linq2Rest.Reactive;
using Linq2Rest.Reactive.Implementations;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive.Implementations
{
    /// <summary>This class contains parameterized unit tests for AsyncXmlRestClientFactory</summary>
    [PexClass(typeof(AsyncXmlRestClientFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class AsyncXmlRestClientFactoryTests
    {
        /// <summary>Test stub for .ctor(Uri)</summary>
		[PexMethod(MaxConditions = 1000)]
        public AsyncXmlRestClientFactory Constructor(Uri serviceBase)
        {
            AsyncXmlRestClientFactory target = new AsyncXmlRestClientFactory(serviceBase);
            return target;
            // TODO: add assertions to method AsyncXmlRestClientFactoryTests.Constructor(Uri)
        }

        /// <summary>Test stub for Create(Uri)</summary>
        [PexMethod]
        public IAsyncRestClient Create(
            [PexAssumeUnderTest]AsyncXmlRestClientFactory target,
            Uri source
        )
        {
            IAsyncRestClient result = target.Create(source);
            return result;
            // TODO: add assertions to method AsyncXmlRestClientFactoryTests.Create(AsyncXmlRestClientFactory, Uri)
        }
    }
}
