// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;

	internal class LongExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(long);
			}
		}

		public ConstantExpression Convert(string token)
		{
			long number;
			return long.TryParse(token, out number)
				? Expression.Constant(number)
				: Expression.Constant(default(long));
		}
	}
}