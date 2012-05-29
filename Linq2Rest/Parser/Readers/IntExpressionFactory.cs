// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class IntExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(int);
			}
		}

		public ConstantExpression Convert(string token)
		{
			int number;
			return int.TryParse(token, out number)
				? Expression.Constant(number)
				: Expression.Constant(default(int));
		}
	}
}