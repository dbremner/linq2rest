// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !SILVERLIGHT
	using System.Diagnostics.Contracts;
#endif

#if !SILVERLIGHT
	[ContractClass(typeof(ValueWriterContracts))]
#endif
	internal interface IValueWriter
	{
		Type Handles { get; }

		string Write(object value);
	}

#if !SILVERLIGHT
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
			throw new NotImplementedException();
		}
	}
#endif
}