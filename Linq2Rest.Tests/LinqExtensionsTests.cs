// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests
{
	using System;
	using System.Reflection;
	using NUnit.Framework;

	[TestFixture]
	public class LinqExtensionsTests
	{
		[Test]
		public void WhenCreatingDynamicTypeWithNullFiledsThenThrows()
		{
			PropertyInfo[] propertyInfos = null;
			Assert.Throws<ArgumentNullException>(() => propertyInfos.GetDynamicType());
		}

		[Test]
		public void WhenCreatingDynamicTypeWithNoPropertiesThenThrows()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new PropertyInfo[0].GetDynamicType());
		}

		[Test]
		public void WhenCreatingDynamicTypeWithOnePropertyInfoThenGettingValueReturnsSetValue()
		{
			var expected = DateTime.UtcNow;
			var properties = new[] { typeof(FakeItem).GetProperty("DateValue") };

			var dynamicType = properties.GetDynamicType();

			dynamic instance = Activator.CreateInstance(dynamicType);
			instance.DateValue = expected;

			Assert.AreEqual(expected, instance.DateValue);
		}

		[Test]
		public void WhenCreatingDynamicTypeWithOnePropertyInfoThenCreatesTypeWithOneProperty()
		{
			var properties = new[] { typeof(FakeItem).GetProperty("DateValue") };

			var dynamicType = properties.GetDynamicType();

			Assert.AreEqual(1, dynamicType.GetProperties().Length);
			Assert.NotNull(dynamicType.GetProperty("DateValue"));
		}

		[Test]
		public void WhenCreatingDynamicTypeWithOnePropertyInfoThenCreatesTypeWithOnePropertyWhereTypeMatchesProperty()
		{
			var properties = new[] { typeof(FakeItem).GetProperty("DateValue") };

			var dynamicType = properties.GetDynamicType();
			var property = dynamicType.GetProperty("DateValue");

			Assert.AreEqual(typeof(DateTime), property.PropertyType);
		}
	}
}