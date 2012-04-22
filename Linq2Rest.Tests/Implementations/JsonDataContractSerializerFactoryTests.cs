﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Implementations
{
	using System;
	using System.Linq;
	using Linq2Rest.Implementations;
	using NUnit.Framework;

	[TestFixture]
	public class JsonDataContractSerializerFactoryTests
	{
		private JsonDataContractSerializerFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = new JsonDataContractSerializerFactory(Type.EmptyTypes);
		}

		[Test]
		public void WhenCreatingSerializerThenDoesNotReturnNull()
		{
			Assert.NotNull(_factory.Create<SimpleContractItem>());
		}

		[Test]
		public void CreatedSerializerCanDeserializeDataContractType()
		{
			const string Json = "{\"Value\": 2, \"Text\":\"test\"}";

			var serializer = _factory.Create<SimpleContractItem>();

			var deserializedResult = serializer.Deserialize(Json.ToStream());

			Assert.AreEqual(2, deserializedResult.Value);
			Assert.AreEqual("test", deserializedResult.SomeString);
		}

		[Test]
		public void CreatedSerializerCanDeserializeListOfDataContractType()
		{
			const string Json = "[{\"Value\": 2, \"Text\":\"test\"}]";

			var serializer = _factory.Create<SimpleContractItem>();

			var deserializedResult = serializer.DeserializeList(Json.ToStream());

			Assert.AreEqual(1, deserializedResult.Count());
		}
	}
}
