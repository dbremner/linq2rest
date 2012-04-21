// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class SingleExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(float);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var number = float.Parse(token.Trim('F', 'f'), NumberStyles.Any);
			return Expression.Constant(number);
		}
	}
}