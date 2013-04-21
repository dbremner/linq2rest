// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringToUpperMethodWriter.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the StringToUpperMethodWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;

	internal class StringToUpperMethodWriter : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
			return expression.Method.DeclaringType == typeof(string)
				   && (expression.Method.Name == "ToUpper" || expression.Method.Name == "ToUpperInvariant");
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
			var obj = expression.Object;
#if !WINDOWS_PHONE
			Contract.Assume(obj != null);
#endif

			return string.Format("toupper({0})", expressionWriter(obj));
		}
	}
}