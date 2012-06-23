// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class ByteExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(byte);
			}
		}

		public ConstantExpression Convert(string token)
		{
			byte number;
			return Expression.Constant(
				byte.TryParse(token, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number)
				? number
				: default(byte));
		}
	}
}