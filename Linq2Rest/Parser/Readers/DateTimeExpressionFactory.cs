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

	internal class DateTimeExpressionFactory : IValueExpressionFactory
	{
		private static readonly Regex _dateTimeRegex = new Regex(@"datetime['\""](\d{4}\-\d{2}\-\d{2}(T\d{2}\:\d{2}\:\d{2})?Z)['\""]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public Type Handles
		{
			get
			{
				return typeof(DateTime);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var match = _dateTimeRegex.Match(token);
			if (match.Success)
			{
				var dateTime = XmlConvert.ToDateTime(match.Groups[1].Value, XmlDateTimeSerializationMode.Utc); // DateTime.Parse(match.Groups[1].Value).ToUniversalTime();
				return Expression.Constant(dateTime);
			}

			return Expression.Constant(default(DateTime));
		}
	}
}