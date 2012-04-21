namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;

	internal interface IValueExpressionFactory
	{
		Type Handles { get; }

		ConstantExpression Convert(string token);
	}
}