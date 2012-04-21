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
			var number = byte.Parse(token, NumberStyles.HexNumber);

			return Expression.Constant(number);
		}
	}
}