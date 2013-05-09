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
	using System.Diagnostics.Contracts;
	using System.Reflection;

	[ContractClass(typeof(ValueWriterContracts))]
	internal interface IValueWriter
	{
		Type Handles { get; }

		string Write(object value);
	}

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
#if NETFX_CORE
			Contract.Requires(Handles.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()));
#else
			Contract.Requires(Handles.IsAssignableFrom(value.GetType()));
#endif
			throw new NotImplementedException();
		}
	}
}