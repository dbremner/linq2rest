// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyAnyMethodWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the EmptyAnyMethodWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.Linq.Expressions;

	internal class EmptyAnyMethodWriter : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
			return expression.Method.Name == "Any" && expression.Arguments.Count == 1;
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
			var firstArg = expressionWriter(expression.Arguments[0]);
			return string.Format("{0}/any()", firstArg);
		}
	}
}