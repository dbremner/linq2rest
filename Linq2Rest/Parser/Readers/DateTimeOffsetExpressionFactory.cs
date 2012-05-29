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

	internal class DateTimeOffsetExpressionFactory : IValueExpressionFactory
	{
		private static readonly Regex _dateTimeOffsetRegex = new Regex(@"datetimeoffset['\""](\d{4}\-\d{2}\-\d{2}(T\d{2}\:\d{2}\:\d{2})?[\-\+]\d{2}:\d{2})['\""]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public Type Handles
		{
			get
			{
				return typeof(DateTimeOffset);
			}
		}

		public ConstantExpression Convert(string token)
		{
			var match = _dateTimeOffsetRegex.Match(token);
			if (match.Success)
			{
				var dateTimeOffset = XmlConvert.ToDateTimeOffset(match.Groups[1].Value);
				return Expression.Constant(dateTimeOffset);
			}

			return Expression.Constant(default(DateTimeOffset));
		}
	}
}