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
			float number;
			return float.TryParse(token.Trim('F', 'f'), NumberStyles.Any, CultureInfo.InvariantCulture, out number)
				? Expression.Constant(number)
				: Expression.Constant(default(float));
		}
	}
}