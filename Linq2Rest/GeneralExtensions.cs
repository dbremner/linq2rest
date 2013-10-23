// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the GeneralExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Text;

	internal static class GeneralExtensions
	{
		private static readonly ConcurrentDictionary<Type, PropertyInfo[]> KnownProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();
		private static readonly ConcurrentDictionary<Type, bool> KnownAnonymousTypes = new ConcurrentDictionary<Type, bool>();

		public static bool IsAnonymousType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return KnownAnonymousTypes.GetOrAdd(
				type,
				t => Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute), false)
						&& t.IsGenericType
						&& t.Name.Contains("AnonymousType") && (t.Name.StartsWith("<>") || t.Name.StartsWith("VB$"))
						&& (t.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic);
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

		public static PropertyInfo[] GetPublicProperties(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return KnownProperties.GetOrAdd(
				type,
				t =>
				{
					if (t.IsInterface)
					{
						var propertyInfos = new List<PropertyInfo>();

						var considered = new List<Type>();
						var queue = new Queue<Type>();
						considered.Add(t);
						queue.Enqueue(t);
						while (queue.Count > 0)
						{
							var subType = queue.Dequeue();
							foreach (var subInterface in subType.GetInterfaces()
								.Where(x => !considered.Contains(x)))
							{
								considered.Add(subInterface);
								queue.Enqueue(subInterface);
							}

							var typeProperties = subType.GetProperties(
								BindingFlags.FlattenHierarchy
								| BindingFlags.Public
								| BindingFlags.Instance);

							var newPropertyInfos = typeProperties
								.Where(x => !propertyInfos.Contains(x));

							propertyInfos.InsertRange(0, newPropertyInfos);
						}

						return propertyInfos.ToArray();
					}

					return t.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
				});
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