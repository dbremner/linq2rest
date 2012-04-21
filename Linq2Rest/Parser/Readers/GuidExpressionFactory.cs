namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;
	using System.Text.RegularExpressions;

	internal class GuidExpressionFactory : IValueExpressionFactory
	{
		private static readonly Regex GuidRegex = new Regex(@"guid['\""]([a-f0-9\-]+)['\""]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public Type Handles
		{
			get
			{
				return typeof(Guid);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var match = GuidRegex.Match(token);
			if (match.Success)
			{
				var guid = Guid.Parse(match.Groups[1].Value);
				return Expression.Constant(guid);
			}
			
			throw new InvalidOperationException("Filter is not recognized as Guid: " + token);
		}
	}
}