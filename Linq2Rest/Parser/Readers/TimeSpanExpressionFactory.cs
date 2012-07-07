// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;
	using System.Text.RegularExpressions;
	using System.Xml;

	internal class TimeSpanExpressionFactory : IValueExpressionFactory
	{
		private static readonly Regex TimeSpanRegex = new Regex(@"^time['\""](P.+)['\""]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public Type Handles
		{
			get
			{
				return typeof(TimeSpan);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var match = TimeSpanRegex.Match(token);
			if (match.Success)
			{
				try
				{
					var timespan = XmlConvert.ToTimeSpan(match.Groups[1].Value);
					return Expression.Constant(timespan);
				}
				catch
				{
					throw new FormatException("Could not read " + token + " as TimeSpan.");
				}
			}

			throw new FormatException("Could not read " + token + " as TimeSpan.");
		}
	}
}
