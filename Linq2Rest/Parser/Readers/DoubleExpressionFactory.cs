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
			var number = double.Parse(token.Trim('D', 'd'), NumberStyles.Any);
			return Expression.Constant(number);
		}
	}
}