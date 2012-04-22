// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Mvc
{
	using System;
	using System.Linq;
	using Linq2Rest.Mvc.Provider;
	using NUnit.Framework;

	[TestFixture]
	public class RuntimeAnonymousTypeSerializerTests
	{
		[Test]
		public void CanDeserializeAnonymousTypeWithTwoProperties()
		{
			const string Json = "{\"Title\":\"blah\", \"Value\":2}";
			var source = new[] { new Tuple<string, int>("test", 1), };
			var anonymousType = source.Select(x => new { Title = x.Item1, Value = x.Item2 }).First();

			var serializerType = typeof(RuntimeAnonymousTypeSerializer<>).MakeGenericType(anonymousType.GetType());
			var serializer = Activator.CreateInstance(serializerType);

			var deserializeMethod = serializerType
				.GetMethods()
				.First(m => m.Name == "Deserialize" && m.ReturnType == anonymousType.GetType());

			var stream = Json.ToStream();
			dynamic result = deserializeMethod.Invoke(serializer, new object[] { stream });

			Assert.AreEqual("blah", result.Title);
		}
		
		[Test]
		public void WhenDeserializingEmptyArrayResponseThenReturnsEmptyList()
		{
			const string Json = "[]";

			var serializer = new RuntimeAnonymousTypeSerializer<FakeItem>();
			var result = serializer.DeserializeList(Json.ToStream()).ToList();
			
			Assert.IsEmpty(result);
		}
		
		[Test]
		public void WhenDeserializingNullResponseThenReturnsEmptyList()
		{
			const string Json = "null";

			var serializer = new RuntimeAnonymousTypeSerializer<FakeItem>();
			var result = serializer.DeserializeList(Json.ToStream()).ToList();
			
			Assert.IsEmpty(result);
		}
	}
}
