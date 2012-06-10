// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	internal static class ParameterValueReader
	{
		private static readonly ConcurrentDictionary<Type, MethodInfo> ParseMethods = new ConcurrentDictionary<Type, MethodInfo>();
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

			if (string.Equals(token, "null", StringComparison.OrdinalIgnoreCase))
			{
				return Expression.Constant(null);
			}

			var factory = ExpressionFactories.FirstOrDefault(x => x.Handles == type);

			return factory == null
				? GetKnownConstant(type, token, formatProvider)
				: factory.Convert(token);
		}

		private static MethodInfo ResolveParseMethod(Type type)
		{
			Contract.Requires(type != null);

			return type.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.Where(x => x.Name == "Parse" && x.GetParameters().Length == 2)
				.FirstOrDefault(x => x.GetParameters().First().ParameterType == typeof(string) && x.GetParameters().ElementAt(1).ParameterType == typeof(IFormatProvider));
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

			var parseMethod = ParseMethods.GetOrAdd(type, ResolveParseMethod);
			if (parseMethod != null)
			{
				var parseResult = parseMethod.Invoke(null, new object[] { token, formatProvider });
				return Expression.Constant(parseResult);
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