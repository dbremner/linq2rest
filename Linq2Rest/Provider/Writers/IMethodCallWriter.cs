// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodCallWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the IMethodCallWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;

#if !WINDOWS_PHONE
	[ContractClass(typeof(MethodCallWriterContracts))]
#endif
	internal interface IMethodCallWriter
	{
		bool CanHandle(MethodCallExpression expression);

		string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter);
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(IMethodCallWriter))]
#endif
	internal abstract class MethodCallWriterContracts : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
#if !WINDOWS_PHONE
			Contract.Requires(expression != null);
#endif
			throw new NotImplementedException();
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
#if !WINDOWS_PHONE
			Contract.Requires(expression != null);
			Contract.Requires(expressionWriter != null);
			Contract.Ensures(Contract.Result<string>() != null);
#endif

			throw new NotImplementedException();
		}
	}
}