// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnyAllMethodWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the AnyAllMethodWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq;
	using System.Linq.Expressions;

	internal class AnyAllMethodWriter : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
			return expression.Method.Name == "Any" || expression.Method.Name == "All";
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
#if !WINDOWS_PHONE
			Contract.Assume(expression.Arguments.Count > 1);
#endif

			var firstArg = expressionWriter(expression.Arguments[0]);
			var method = expression.Method.Name.ToLowerInvariant();
			var parameter = expression.Arguments[1] is LambdaExpression ? (expression.Arguments[1] as LambdaExpression).Parameters.First().Name : null;
			var predicate = expressionWriter(expression.Arguments[1]);

			return string.Format("{0}/{1}({2}: {3})", firstArg, method, parameter, predicate);
		}
	}
}