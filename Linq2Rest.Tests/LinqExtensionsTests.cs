// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Tests
{
	using System;
	using System.Reflection;

	using NUnit.Framework;

	using UrlQueryParser.Parser;

	public class LinqExtensionsTests
	{
		[Test]
		public void WhenCreatingDynamicTypeWithNullFiledsThenThrows()
		{
			PropertyInfo[] propertyInfos = null;
			Assert.Throws<ArgumentNullException>(() => propertyInfos.GetDynamicType());
		}

		[Test]
		public void WhenCreatingDynamicTypeWithNoFieldsThenThrows()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new PropertyInfo[0].GetDynamicType());
		}

		[Test]
		public void WhenCreatingDynamicTypeWithOnePropertyInfoThenCreatesTypeWithOneField()
		{
			var properties = new[] { typeof(FakeItem).GetProperty("DateValue") };

			var dynamicType = properties.GetDynamicType();

			Assert.AreEqual(1, dynamicType.GetFields().Length);
			Assert.NotNull(dynamicType.GetField("DateValue"));
		}

		[Test]
		public void WhenCreatingDynamicTypeWithOnePropertyInfoThenCreatesTypeWithOneFieldWhereTypeMatchesProperty()
		{
			var properties = new[] { typeof(FakeItem).GetProperty("DateValue") };

			var dynamicType = properties.GetDynamicType();
			var field = dynamicType.GetField("DateValue");

			Assert.AreEqual(typeof(DateTime), field.FieldType);
		}
	}
}