// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanValueWriter.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TimeSpanValueWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Xml;

	internal class TimeSpanValueWriter : IValueWriter
	{
		public Type Handles
		{
			get
			{
				return typeof(TimeSpan);
			}
		}

		public string Write(object value)
		{
			return string.Format("time'{0}'", XmlConvert.ToString((TimeSpan)value));
		}
	}
}