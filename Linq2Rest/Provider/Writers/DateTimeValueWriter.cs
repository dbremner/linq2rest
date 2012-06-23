// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Xml;

	internal class DateTimeValueWriter : IValueWriter
	{
		public Type Handles
		{
			get
			{
				return typeof(DateTime);
			}
		}

		public string Write(object value)
		{
			var dateTimeValue = (DateTime)value;

			return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
		}
	}
}
