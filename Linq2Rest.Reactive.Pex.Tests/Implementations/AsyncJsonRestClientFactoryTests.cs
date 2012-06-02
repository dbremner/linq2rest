// <copyright file="AsyncJsonRestClientFactoryTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using System;
using Linq2Rest.Reactive;
using Linq2Rest.Reactive.Implementations;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest.Reactive.Implementations
{
    /// <summary>This class contains parameterized unit tests for AsyncJsonRestClientFactory</summary>
    [PexClass(typeof(AsyncJsonRestClientFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class AsyncJsonRestClientFactoryTests
    {
        /// <summary>Test stub for .ctor(Uri)</summary>
		[PexMethod(MaxConditions = 500)]
        public AsyncJsonRestClientFactory Constructor(Uri serviceBase)
        {
            AsyncJsonRestClientFactory target = new AsyncJsonRestClientFactory(serviceBase);
            return target;
            // TODO: add assertions to method AsyncJsonRestClientFactoryTests.Constructor(Uri)
        }

        /// <summary>Test stub for Create(Uri)</summary>
        [PexMethod]
        public IAsyncRestClient Create(
            [PexAssumeUnderTest]AsyncJsonRestClientFactory target,
            Uri source
        )
        {
            IAsyncRestClient result = target.Create(source);
            return result;
            // TODO: add assertions to method AsyncJsonRestClientFactoryTests.Create(AsyncJsonRestClientFactory, Uri)
        }
    }
}
