// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Linq.Expressions;
	using System.Text.RegularExpressions;

	internal class BooleanExpressionFactory : IValueExpressionFactory
	{
		private static readonly Regex _trueRegex = new Regex("1|true", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _falseRegex = new Regex("0|false", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public Type Handles
		{
			get
			{
				return typeof(bool);
			}
		}

		public ConstantExpression Convert(string token)
		{
			if (_trueRegex.IsMatch(token))
			{
				return Expression.Constant(true);
			}

			if (_falseRegex.IsMatch(token))
			{
				return Expression.Constant(false);
			}

			throw new InvalidOperationException("Filter is not recognized as Boolean: " + token);
		}
	}
}