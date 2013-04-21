// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathMethodWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the MathMethodWriter type.
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
	[ContractClass(typeof(MathMethodWriterContracts))]
#endif
	internal abstract class MathMethodWriter : IMethodCallWriter
	{
		protected abstract string MethodName { get; }

		public abstract bool CanHandle(MethodCallExpression expression);

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
#if !WINDOWS_PHONE
			Contract.Assume(expression.Arguments.Count > 0);
#endif

			var mathArgument = expression.Arguments[0];

#if !WINDOWS_PHONE
			Contract.Assume(mathArgument != null);
#endif
			return string.Format("{0}({1})", MethodName, expressionWriter(mathArgument));
		}
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(MathMethodWriter))]
#endif
	internal abstract class MathMethodWriterContracts : MathMethodWriter
	{
		protected override string MethodName
		{
			get
			{
#if !WINDOWS_PHONE
				Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
#endif
				throw new NotImplementedException();
			}
		}
	}
}