// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;

	[ContractClass(typeof(ValueExpressionFactoryContracts))]
	internal interface IValueExpressionFactory
	{
		Type Handles { get; }

		ConstantExpression Convert(string token);
	}

	[ContractClassFor(typeof(IValueExpressionFactory))]
	internal abstract class ValueExpressionFactoryContracts : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				Contract.Ensures(Contract.Result<Type>() != null);
				throw new NotImplementedException();
			}
		}

		public ConstantExpression Convert(string token)
		{
			Contract.Requires(token != null);
			Contract.Ensures(Contract.Result<ConstantExpression>() != null);
			throw new NotImplementedException();
		}
	}
}