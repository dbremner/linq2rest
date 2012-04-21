namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Globalization;
	using System.Linq.Expressions;

	internal class ShortExpressionFactory : IValueExpressionFactory
	{
		public Type Handles
		{
			get
			{
				return typeof(short);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var number = short.Parse(token, NumberStyles.Integer);

			return Expression.Constant(number);
		}
	}
}