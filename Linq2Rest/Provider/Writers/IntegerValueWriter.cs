// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerValueWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the IntegerValueWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;

	internal abstract class IntegerValueWriter : IValueWriter
	{
		public abstract Type Handles { get; }

		public string Write(object value)
		{
			return value.ToString();
		}
	}
}