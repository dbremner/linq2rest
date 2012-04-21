// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	internal static class ParameterValueWriter
	{
		private static readonly IList<IValueWriter> ValueWriters;

		static ParameterValueWriter()
		{
			ValueWriters = new List<IValueWriter>
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
								new GuidValueWriter()
							};
		}

		public static string Write(object value)
		{
			if (value == null)
			{
				return "null";
			}

			var type = value.GetType();

			if (type.IsEnum)
			{
				return value.ToString();
			}

			var writer = ValueWriters.FirstOrDefault(x => x.Handles == type);

			if (writer != null)
			{
				return writer.Write(value);
			}

			if (typeof(Nullable<>).IsAssignableFrom(type))
			{
				var genericParameter = type.GetGenericArguments()[0];

				return Write(Convert.ChangeType(value, genericParameter, CultureInfo.CurrentCulture));
			}

			return value.ToString();
		}
	}
}
