// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class DoubleExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(double);
			}
		}

		public ConstantExpression Convert(string token)
		{
			double number;
			return double.TryParse(token.Trim('D', 'd'), NumberStyles.Any, CultureInfo.InvariantCulture, out number)
				? Expression.Constant(number)
				: Expression.Constant(default(double));
		}
	}
}