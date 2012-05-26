// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	internal static class MethodProvider
	{
		private static readonly ConstantExpression _innerIgnoreCaseExpression;
		private static readonly MethodInfo _innerContainsMethod;
		private static readonly MethodInfo _innerIndexOfMethod;
		private static readonly MethodInfo _endsWithMethod1;
		private static readonly MethodInfo _startsWithMethod;
		private static readonly PropertyInfo _lengthProperty;
		private static readonly MethodInfo _substringMethod;
		private static readonly MethodInfo _toLowerMethod;
		private static readonly MethodInfo _toUpperMethod;
		private static readonly MethodInfo _trimMethod1;
		private static readonly PropertyInfo _dayProperty;
		private static readonly PropertyInfo _hourProperty;
		private static readonly PropertyInfo _minuteProperty;
		private static readonly PropertyInfo _secondProperty;
		private static readonly PropertyInfo _monthProperty;
		private static readonly PropertyInfo _yearProperty;
		private static readonly MethodInfo _doubleRoundMethod;
		private static readonly MethodInfo _decimalRoundMethod;
		private static readonly MethodInfo _doubleFloorMethod;
		private static readonly MethodInfo _decimalFloorMethod;
		private static readonly MethodInfo _doubleCeilingMethod;
		private static readonly MethodInfo _decimalCeilingMethod;

		static MethodProvider()
		{
			var stringType = typeof(string);
			var datetimeType = typeof(DateTime);
			var mathType = typeof(Math);
			var stringComparisonType = typeof(StringComparison);

			_innerIgnoreCaseExpression = Expression.Constant(StringComparison.OrdinalIgnoreCase);

			_innerContainsMethod = stringType.GetMethod("Contains", new[] { stringType });
			_innerIndexOfMethod = stringType.GetMethod("IndexOf", new[] { stringType, stringComparisonType });
			_endsWithMethod1 = stringType.GetMethod("EndsWith", new[] { stringType, stringComparisonType });
			_startsWithMethod = stringType.GetMethod("StartsWith", new[] { stringType, stringComparisonType });
			_lengthProperty = stringType.GetProperty("Length", Type.EmptyTypes);
			_substringMethod = stringType.GetMethod("Substring", new[] { typeof(int) });
			_toLowerMethod = stringType.GetMethod("ToLowerInvariant", Type.EmptyTypes);
			_toUpperMethod = stringType.GetMethod("ToUpperInvariant", Type.EmptyTypes);
			_trimMethod1 = stringType.GetMethod("Trim", Type.EmptyTypes);

			_dayProperty = datetimeType.GetProperty("Day", Type.EmptyTypes);
			_hourProperty = datetimeType.GetProperty("Hour", Type.EmptyTypes);
			_minuteProperty = datetimeType.GetProperty("Minute", Type.EmptyTypes);
			_secondProperty = datetimeType.GetProperty("Second", Type.EmptyTypes);
			_monthProperty = datetimeType.GetProperty("Month", Type.EmptyTypes);
			_yearProperty = datetimeType.GetProperty("Year", Type.EmptyTypes);

			_doubleRoundMethod = mathType.GetMethod("Round", new[] { typeof(double) });
			_decimalRoundMethod = mathType.GetMethod("Round", new[] { typeof(decimal) });
			_doubleFloorMethod = mathType.GetMethod("Floor", new[] { typeof(double) });
			_decimalFloorMethod = mathType.GetMethod("Floor", new[] { typeof(decimal) });
			_doubleCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(double) });
			_decimalCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(decimal) });
		}

		public static ConstantExpression IgnoreCaseExpression
		{
			get
			{
				return _innerIgnoreCaseExpression;
			}
		}

		public static MethodInfo IndexOfMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _innerIndexOfMethod;
			}
		}

		public static MethodInfo ContainsMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _innerContainsMethod;
			}
		}

		public static MethodInfo EndsWithMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _endsWithMethod1;
			}
		}

		public static MethodInfo StartsWithMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _startsWithMethod;
			}
		}

		public static PropertyInfo LengthProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _lengthProperty;
			}
		}

		public static MethodInfo SubstringMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _substringMethod;
			}
		}

		public static MethodInfo ToLowerMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _toLowerMethod;
			}
		}

		public static MethodInfo ToUpperMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _toUpperMethod;
			}
		}

		public static MethodInfo TrimMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _trimMethod1;
			}
		}

		public static PropertyInfo DayProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _dayProperty;
			}
		}

		public static PropertyInfo HourProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _hourProperty;
			}
		}

		public static PropertyInfo MinuteProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _minuteProperty;
			}
		}

		public static PropertyInfo SecondProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _secondProperty;
			}
		}

		public static PropertyInfo MonthProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _monthProperty;
			}
		}

		public static PropertyInfo YearProperty
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfo>() != null);
				return _yearProperty;
			}
		}

		public static MethodInfo DoubleRoundMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _doubleRoundMethod;
			}
		}

		public static MethodInfo DecimalRoundMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _decimalRoundMethod;
			}
		}

		public static MethodInfo DoubleFloorMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _doubleFloorMethod;
			}
		}

		public static MethodInfo DecimalFloorMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _decimalFloorMethod;
			}
		}

		public static MethodInfo DoubleCeilingMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _doubleCeilingMethod;
			}
		}

		public static MethodInfo DecimalCeilingMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
				return _decimalCeilingMethod;
			}
		}

		public static MethodInfo GetAnyAllMethod(string name, Type collectionType)
		{
			Contract.Requires(collectionType != null);

			var implementationType = GetIEnumerableImpl(collectionType);

			var elemType = implementationType.GetGenericArguments()[0];
			var predType = typeof(Func<,>).MakeGenericType(elemType, typeof(bool));

			var allMethod = (MethodInfo)GetGenericMethod(
														 typeof(Enumerable),
														 name,
														 new[] { elemType },
														 new[] { implementationType, predType },
														 BindingFlags.Static);

			return allMethod;
		}

		public static Type GetIEnumerableImpl(Type type)
		{
			Contract.Requires(type != null);

			// Get IEnumerable implementation. Either type is IEnumerable<T> for some T, 
			// or it implements IEnumerable<T> for some T. We need to find the interface.
			if (IsIEnumerable(type))
			{
				return type;
			}

			var interfaces = type.FindInterfaces((m, o) => IsIEnumerable(m), null);
			
			Contract.Assume(interfaces.Count() > 0);
			
			var t = interfaces.First();

			return t;
		}

		private static MethodBase GetGenericMethod(Type type, string name, Type[] typeArgs, Type[] argTypes, BindingFlags flags)
		{
			Contract.Requires(typeArgs != null);
			Contract.Requires(type != null);

			var typeArity = typeArgs.Length;
			var methods = type.GetMethods()
				.Where(m => m.Name == name)
				.Where(m => m.GetGenericArguments().Length == typeArity)
				.Select(m => m.MakeGenericMethod(typeArgs));

			return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null);
		}

		private static bool IsIEnumerable(Type type)
		{
			Contract.Requires(type != null);

			return type.IsGenericType
				&& type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
		}
	}
}