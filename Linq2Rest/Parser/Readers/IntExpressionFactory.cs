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
			var number = int.Parse(token, NumberStyles.Integer);

			return Expression.Constant(number);
		}
	}
}