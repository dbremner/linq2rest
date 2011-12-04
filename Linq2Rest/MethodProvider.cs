// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;

	public static class MethodProvider
	{
		public static readonly ConstantExpression IgnoreCaseExpression;

		private static readonly MethodInfo InnerChangeTypeMethod;
		private static readonly MethodInfo InnerContainsMethod;
		private static readonly MethodInfo InnerIndexOfMethod;
		private static readonly MethodInfo EndsWithMethod1;
		private static readonly MethodInfo StartsWithMethod1;
		private static readonly PropertyInfo LengthProperty1;
		private static readonly MethodInfo SubstringMethod1;
		private static readonly MethodInfo ToLowerMethod1;
		private static readonly MethodInfo ToUpperMethod1;
		private static readonly MethodInfo TrimMethod1;
		private static readonly PropertyInfo DayProperty1;
		private static readonly PropertyInfo HourProperty1;
		private static readonly PropertyInfo MinuteProperty1;
		private static readonly PropertyInfo SecondProperty1;
		private static readonly PropertyInfo MonthProperty1;
		private static readonly PropertyInfo YearProperty1;
		private static readonly MethodInfo DoubleRoundMethod1;
		private static readonly MethodInfo DecimalRoundMethod1;
		private static readonly MethodInfo DoubleFloorMethod1;
		private static readonly MethodInfo DecimalFloorMethod1;
		private static readonly MethodInfo DoubleCeilingMethod1;
		private static readonly MethodInfo DecimalCeilingMethod1;

		static MethodProvider()
		{
			var stringType = typeof(string);
			var datetimeType = typeof(DateTime);
			var mathType = typeof(Math);
			var stringComparisonType = typeof(StringComparison);

			InnerChangeTypeMethod = typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });
			IgnoreCaseExpression = Expression.Constant(StringComparison.OrdinalIgnoreCase);

			InnerContainsMethod = stringType.GetMethod("Contains", new[] { stringType });
			InnerIndexOfMethod = stringType.GetMethod("IndexOf", new[] { stringType, stringComparisonType });
			EndsWithMethod1 = stringType.GetMethod("EndsWith", new[] { stringType, stringComparisonType });
			StartsWithMethod1 = stringType.GetMethod("StartsWith", new[] { stringType, stringComparisonType });
			LengthProperty1 = stringType.GetProperty("Length", Type.EmptyTypes);
			SubstringMethod1 = stringType.GetMethod("Substring", new[] { typeof(int) });
			ToLowerMethod1 = stringType.GetMethod("ToLowerInvariant", Type.EmptyTypes);
			ToUpperMethod1 = stringType.GetMethod("ToUpperInvariant", Type.EmptyTypes);
			TrimMethod1 = stringType.GetMethod("Trim", Type.EmptyTypes);

			DayProperty1 = datetimeType.GetProperty("Day", Type.EmptyTypes);
			HourProperty1 = datetimeType.GetProperty("Hour", Type.EmptyTypes);
			MinuteProperty1 = datetimeType.GetProperty("Minute", Type.EmptyTypes);
			SecondProperty1 = datetimeType.GetProperty("Second", Type.EmptyTypes);
			MonthProperty1 = datetimeType.GetProperty("Month", Type.EmptyTypes);
			YearProperty1 = datetimeType.GetProperty("Year", Type.EmptyTypes);

			DoubleRoundMethod1 = mathType.GetMethod("Round", new[] { typeof(double) });
			DecimalRoundMethod1 = mathType.GetMethod("Round", new[] { typeof(decimal) });
			DoubleFloorMethod1 = mathType.GetMethod("Floor", new[] { typeof(double) });
			DecimalFloorMethod1 = mathType.GetMethod("Floor", new[] { typeof(decimal) });
			DoubleCeilingMethod1 = mathType.GetMethod("Ceiling", new[] { typeof(double) });
			DecimalCeilingMethod1 = mathType.GetMethod("Ceiling", new[] { typeof(decimal) });
		}

		public static MethodInfo ChangeTypeMethod
		{
			get { return InnerChangeTypeMethod; }
		}

		public static MethodInfo IndexOfMethod
		{
			get { return InnerIndexOfMethod; }
		}

		public static MethodInfo ContainsMethod
		{
			get { return InnerContainsMethod; }
		}

		public static MethodInfo EndsWithMethod
		{
			get { return EndsWithMethod1; }
		}

		public static MethodInfo StartsWithMethod
		{
			get
			{
				return StartsWithMethod1;
			}
		}

		public static PropertyInfo LengthProperty
		{
			get
			{
				return LengthProperty1;
			}
		}

		public static MethodInfo SubstringMethod
		{
			get
			{
				return SubstringMethod1;
			}
		}

		public static MethodInfo ToLowerMethod
		{
			get
			{
				return ToLowerMethod1;
			}
		}

		public static MethodInfo ToUpperMethod
		{
			get
			{
				return ToUpperMethod1;
			}
		}

		public static MethodInfo TrimMethod
		{
			get
			{
				return TrimMethod1;
			}
		}

		public static PropertyInfo DayProperty
		{
			get
			{
				return DayProperty1;
			}
		}

		public static PropertyInfo HourProperty
		{
			get
			{
				return HourProperty1;
			}
		}

		public static PropertyInfo MinuteProperty
		{
			get
			{
				return MinuteProperty1;
			}
		}

		public static PropertyInfo SecondProperty
		{
			get
			{
				return SecondProperty1;
			}
		}

		public static PropertyInfo MonthProperty
		{
			get
			{
				return MonthProperty1;
			}
		}

		public static PropertyInfo YearProperty
		{
			get
			{
				return YearProperty1;
			}
		}

		public static MethodInfo DoubleRoundMethod
		{
			get
			{
				return DoubleRoundMethod1;
			}
		}

		public static MethodInfo DecimalRoundMethod
		{
			get
			{
				return DecimalRoundMethod1;
			}
		}

		public static MethodInfo DoubleFloorMethod
		{
			get
			{
				return DoubleFloorMethod1;
			}
		}

		public static MethodInfo DecimalFloorMethod
		{
			get
			{
				return DecimalFloorMethod1;
			}
		}

		public static MethodInfo DoubleCeilingMethod
		{
			get
			{
				return DoubleCeilingMethod1;
			}
		}

		public static MethodInfo DecimalCeilingMethod
		{
			get
			{
				return DecimalCeilingMethod1;
			}
		}
	}
}