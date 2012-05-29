// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;

	internal class UnsignedIntExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(uint);
			}
		}

		public ConstantExpression Convert(string token)
		{
			uint number;
			return uint.TryParse(token, out number)
				? Expression.Constant(number)
				: Expression.Constant(default(uint));
		}
	}
}