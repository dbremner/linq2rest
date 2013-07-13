// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntValueWriter.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the IntValueWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;

	internal class IntValueWriter : IntegerValueWriter
	{
		public override Type Handles
		{
			get
			{
				return typeof(int);
			}
		}
	}
}