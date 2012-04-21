namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
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
			var number = uint.Parse(token, NumberStyles.Integer);

			return Expression.Constant(number);
		}
	}
}