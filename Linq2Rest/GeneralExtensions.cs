// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest
{
	using System;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Text;

	internal static class GeneralExtensions
	{
		public static bool IsAnonymousType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
				&& type.IsGenericType
				&& type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
				&& (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
		}

		public static string Capitalize(this string input)
		{
			Contract.Requires(!string.IsNullOrEmpty(input));

			return char.ToUpperInvariant(input[0]) + input.Substring(1);
		}

		public static Stream ToStream(this string input)
		{
			Contract.Requires(input != null);

			return new MemoryStream(Encoding.UTF8.GetBytes(input ?? string.Empty));
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, Expression keySelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(keySelector != null);

			var propertyType = keySelector.GetType().GetGenericArguments()[0].GetGenericArguments()[1];
			var orderbyMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);

			Contract.Assume(orderbyMethod != null);

			orderbyMethod = orderbyMethod.MakeGenericMethod(typeof(T), propertyType);

			return (IOrderedQueryable<T>)orderbyMethod.Invoke(null, new object[] { source, keySelector });
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, Expression keySelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(keySelector != null);

			var propertyType = keySelector.GetType().GetGenericArguments()[0].GetGenericArguments()[1];
			var orderbyMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);

			Contract.Assume(orderbyMethod != null);

			orderbyMethod = orderbyMethod.MakeGenericMethod(typeof(T), propertyType);

			return (IOrderedQueryable<T>)orderbyMethod.Invoke(null, new object[] { source, keySelector });
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, Expression keySelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(keySelector != null);

			var propertyType = keySelector.GetType().GetGenericArguments()[0].GetGenericArguments()[1];
			var orderbyMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.Name == "ThenBy" && x.GetParameters().Length == 2);

			Contract.Assume(orderbyMethod != null);

			orderbyMethod = orderbyMethod.MakeGenericMethod(typeof(T), propertyType);

			return (IOrderedQueryable<T>)orderbyMethod.Invoke(null, new object[] { source, keySelector });
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, Expression keySelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(keySelector != null);

			var propertyType = keySelector.GetType().GetGenericArguments()[0].GetGenericArguments()[1];
			var orderbyMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2);

			Contract.Assume(orderbyMethod != null);

			orderbyMethod = orderbyMethod.MakeGenericMethod(typeof(T), propertyType);

			return (IOrderedQueryable<T>)orderbyMethod.Invoke(null, new object[] { source, keySelector });
		}
	}
}