// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the IValueWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE && !NETFX_CORE
	using System.Diagnostics.Contracts;
#endif

#if !WINDOWS_PHONE && !NETFX_CORE
	[ContractClass(typeof(ValueWriterContracts))]
#endif
    internal interface IValueWriter
	{
		Type Handles { get; }

		string Write(object value);
    }

#if !WINDOWS_PHONE && !NETFX_CORE
	[ContractClassFor(typeof(IValueWriter))]
	internal abstract class ValueWriterContracts : IValueWriter
	{
		public Type Handles
		{
			get
			{
				Contract.Ensures(Contract.Result<Type>() != null);

				throw new NotImplementedException();
			}
		}

		public string Write(object value)
		{
			Contract.Requires(value != null);
			Contract.Requires(Handles.IsAssignableFrom(value.GetType()));
			throw new NotImplementedException();
		}
	}
#endif
}