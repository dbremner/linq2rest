namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
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
			var number = long.Parse(token, NumberStyles.Integer);

			return Expression.Constant(number);
		}
	}
}