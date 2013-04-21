// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMethodWriter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DefaultMethodWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider.Writers
{
	using System;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;

	internal class DefaultMethodWriter : IMethodCallWriter
	{
		public bool CanHandle(MethodCallExpression expression)
		{
			return true;
		}

		public string Handle(MethodCallExpression expression, Func<Expression, string> expressionWriter)
		{
			return ParameterValueWriter.Write(GetValue(expression));
		}

		private static object GetValue(Expression input)
		{
#if !WINDOWS_PHONE
			Contract.Requires(input != null);
#endif

			var objectMember = Expression.Convert(input, typeof(object));
			var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

			return getterLambda();
		}

	}
}