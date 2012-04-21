namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.IO;
	using System.Linq.Expressions;

	internal class StreamExpressionFactory : ByteArrayExpressionFactory
	{
		public override Type Handles
		{
			get
			{
				return typeof(Stream);
			}
		}

		public override ConstantExpression Convert(string token)
		{
			var baseResult = base.Convert(token);

			var stream = new MemoryStream((byte[])baseResult.Value);

			return Expression.Constant(stream);
		}
	}
}