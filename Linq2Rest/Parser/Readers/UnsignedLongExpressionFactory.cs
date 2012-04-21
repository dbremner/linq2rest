// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class UnsignedLongExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(ulong);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var number = ulong.Parse(token, NumberStyles.Integer);

			return Expression.Constant(number);
		}
	}
}