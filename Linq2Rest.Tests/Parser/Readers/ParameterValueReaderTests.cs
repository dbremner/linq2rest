namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.IO;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class ParameterValueReaderTests
	{
		[Test]
		public void EnumIsAssignableFromAnEnumType()
		{
			Assert.IsTrue(typeof(Enum).IsAssignableFrom(typeof(Choice)));
		}

		[Test]
		public void DecimalIsNotAssignableFromDouble()
		{
			Assert.False(typeof(decimal).IsAssignableFrom(typeof(double)));
		}

		[TestCase("null", typeof(string))]
		[TestCase("'test'", typeof(string))]
		[TestCase("guid'D81D5F0C-2574-4D5C-A394-E280E6E02A7F'", typeof(Guid))]
		[TestCase("guid'926adbb7-328f-44c5-8900-e8f78e4a66f8'", typeof(Guid))]
		[TestCase("guid'D81D5F0C-2574-4D5C-A394-E280E6E02A7F'", typeof(Guid?))]
		[TestCase("guid'926adbb7-328f-44c5-8900-e8f78e4a66f8'", typeof(Guid?))]
		[TestCase("datetime'2012-04-21T12:34:56Z'", typeof(DateTime))]
		[TestCase("datetime'2012-04-21T12:34:56Z'", typeof(DateTime?))]
		[TestCase("1.23M", typeof(decimal))]
		[TestCase("1.23m", typeof(decimal))]
		[TestCase("1.23", typeof(decimal))]
		[TestCase("1.23D", typeof(double))]
		[TestCase("1.23d", typeof(double))]
		[TestCase("1.23", typeof(double))]
		[TestCase("1.23F", typeof(float))]
		[TestCase("1.23f", typeof(float))]
		[TestCase("1.23", typeof(float))]
		[TestCase("123", typeof(long))]
		[TestCase("123", typeof(int))]
		[TestCase("123", typeof(short))]
		[TestCase("123", typeof(long?))]
		[TestCase("123", typeof(int?))]
		[TestCase("123", typeof(short?))]
		[TestCase("123", typeof(ulong))]
		[TestCase("123", typeof(uint))]
		[TestCase("123", typeof(ushort))]
		[TestCase("123", typeof(ulong?))]
		[TestCase("123", typeof(uint?))]
		[TestCase("123", typeof(ushort?))]
		[TestCase("1.23M", typeof(decimal?))]
		[TestCase("1.23m", typeof(decimal?))]
		[TestCase("1.23", typeof(decimal?))]
		[TestCase("1.23D", typeof(double?))]
		[TestCase("1.23d", typeof(double?))]
		[TestCase("1.23", typeof(double?))]
		[TestCase("1.23F", typeof(float?))]
		[TestCase("1.23f", typeof(float?))]
		[TestCase("1.23", typeof(float?))]
		[TestCase("12", typeof(byte))]
		[TestCase("f6", typeof(byte))]
		[TestCase("12", typeof(byte?))]
		[TestCase("f6", typeof(byte?))]
		[TestCase("true", typeof(bool))]
		[TestCase("false", typeof(bool))]
		[TestCase("1", typeof(bool))]
		[TestCase("0", typeof(bool))]
		[TestCase("true", typeof(bool?))]
		[TestCase("false", typeof(bool?))]
		[TestCase("1", typeof(bool?))]
		[TestCase("0", typeof(bool?))]
		[TestCase("X'ZWFzdXJlLg=='", typeof(byte[]))]
		[TestCase("binary'ZWFzdXJlLg=='", typeof(byte[]))]
		[TestCase("X'ZWFzdXJlLg=='", typeof(Stream))]
		[TestCase("binary'ZWFzdXJlLg=='", typeof(Stream))]
		public void CanConvertValidFilterValue(string token, Type type)
		{
			Assert.DoesNotThrow(() => ParameterValueReader.Read(type, token, CultureInfo.CurrentCulture));
		}
	}
}
