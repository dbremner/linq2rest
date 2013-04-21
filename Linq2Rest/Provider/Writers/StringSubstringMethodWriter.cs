// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSubstringMethodWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the StringSubstringMethodWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;

	internal class StringSubstringMethodWriter : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
			return expression.Method.DeclaringType == typeof(string)
				   && expression.Method.Name == "Substring";
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
#if !WINDOWS_PHONE
			Contract.Assume(expression.Arguments.Count > 0);
#endif
			var obj = expression.Object;
#if !WINDOWS_PHONE
			Contract.Assume(obj != null);
#endif

			if (expression.Arguments.Count == 1)
			{
				var argumentExpression = expression.Arguments[0];

#if !WINDOWS_PHONE
				Contract.Assume(argumentExpression != null);
#endif

				return string.Format(
					"substring({0}, {1})", expressionWriter(obj), expressionWriter(argumentExpression));
			}

			var firstArgument = expression.Arguments[0];
			var secondArgument = expression.Arguments[1];

#if !WINDOWS_PHONE
			Contract.Assume(firstArgument != null);
			Contract.Assume(secondArgument != null);
#endif

			return string.Format(
				"substring({0}, {1}, {2})",
				expressionWriter(obj),
				expressionWriter(firstArgument),
				expressionWriter(secondArgument));
		}
	}
}