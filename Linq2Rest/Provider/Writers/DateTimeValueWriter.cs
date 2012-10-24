// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeValueWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DateTimeValueWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

#if !NETFX_CORE
			return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
#else
			return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue.ToUniversalTime()));
#endif
		}
	}
}
