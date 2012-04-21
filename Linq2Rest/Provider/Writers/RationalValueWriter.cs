// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Globalization;

	internal abstract class RationalValueWriter : IValueWriter
	{
		public abstract Type Handles { get; }

		public string Write(object value)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", value);
		}
	}
}