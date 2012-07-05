// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	internal static class ParameterValueReader
	{
		private static readonly IList<IValueExpressionFactory> ExpressionFactories;

		static ParameterValueReader()
		{
			ExpressionFactories = new List<IValueExpressionFactory>
			                      	{
										new BooleanExpressionFactory(),
										new ByteExpressionFactory(),
			                      		new GuidExpressionFactory(),
										new DateTimeExpressionFactory(),
										new TimeSpanExpressionFactory(),
										new DateTimeOffsetExpressionFactory(),
										new DecimalExpressionFactory(),
										new DoubleExpressionFactory(),
										new SingleExpressionFactory(),
										new ByteArrayExpressionFactory(),
										new StreamExpressionFactory(),
										new LongExpressionFactory(),
										new IntExpressionFactory(),
										new ShortExpressionFactory(),
										new UnsignedIntExpressionFactory(),
										new UnsignedLongExpressionFactory(),
										new UnsignedShortExpressionFactory()
			                      	};
		}

		public static Expression Read(Type type, string token, IFormatProvider formatProvider)
		{
			Contract.Requires(token != null);
			Contract.Requires(type != null);

			var factory = ExpressionFactories.FirstOrDefault(x => x.Handles == type);

			return factory == null
				? GetKnownConstant(type, token, formatProvider)
				: factory.Convert(token);
		}

		private static Expression GetKnownConstant(Type type, string token, IFormatProvider formatProvider)
		{
			Contract.Requires(token != null);
			Contract.Requires(type != null);

			if (type.IsEnum)
			{
				var enumValue = Enum.Parse(type, token, true);
				return Expression.Constant(enumValue);
			}

			if(typeof(IConvertible).IsAssignableFrom(type))
			{
				return Expression.Constant(Convert.ChangeType(token, type, formatProvider), type);
			}

			if (type.IsGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
			{
				var genericTypeArgument = type.GetGenericArguments()[0];
				var value = Read(genericTypeArgument, token, formatProvider);
				if (value != null)
				{
					return Expression.Convert(value, type);
				}
			}

			return null;
		}
	}
}