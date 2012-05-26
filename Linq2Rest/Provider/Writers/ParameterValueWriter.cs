﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;

	internal static class ParameterValueWriter
	{
		private static readonly IList<IValueWriter> _valueWriters;

		static ParameterValueWriter()
		{
			_valueWriters = new List<IValueWriter>
							{
								new StringValueWriter(),
								new BooleanValueWriter(),
								new IntValueWriter(),
								new LongValueWriter(),
								new ShortValueWriter(),
								new UnsignedIntValueWriter(),
								new UnsignedLongValueWriter(),
								new UnsignedShortValueWriter(),
								new ByteArrayValueWriter(),
								new StreamValueWriter(),
								new DecimalValueWriter(),
								new DoubleValueWriter(),
								new SingleValueWriter(),
								new ByteValueWriter(),
								new GuidValueWriter(),
								new DateTimeValueWriter(),
								new TimeSpanValueWriter(),
								new DateTimeOffsetValueWriter()
							};
		}

		public static string Write(object value)
		{
			if (value == null)
			{
				return "null";
			}

#if !NETFX_CORE
			var type = value.GetType();

			if (type.IsEnum)
			{
				return value.ToString();
			}
#else
			var type = value.GetType();
			if (type.GetTypeInfo().IsEnum)
			{
				return value.ToString();
			}
#endif
			var writer = _valueWriters.FirstOrDefault(x => x.Handles == type);

			if (writer != null)
			{
				return writer.Write(value);
			}
			
#if !NETFX_CORE
			if (typeof(Nullable<>).IsAssignableFrom(type))
			{
				var genericParameter = type.GetGenericArguments()[0];

				return Write(Convert.ChangeType(value, genericParameter, CultureInfo.CurrentCulture));
			}
#else
			var typeInfo = type.GetTypeInfo();
			if (typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(typeInfo))
			{
				var genericParameter = typeInfo.GenericTypeArguments[0];

				return Write(Convert.ChangeType(value, genericParameter, CultureInfo.CurrentCulture));
			}
#endif

			return value.ToString();
		}
	}
}
