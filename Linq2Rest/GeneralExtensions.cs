// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
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
	using Linq2Rest.Parser;

	internal static class GeneralExtensions
	{
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
			Contract.Requires(!String.IsNullOrEmpty(input));

			return Char.ToUpperInvariant(input[0]) + input.Substring(1);
		}

		public static Stream ToStream(this string input)
		{
			Contract.Requires(input != null);

			return new MemoryStream(Encoding.UTF8.GetBytes(input ?? String.Empty));
		}

		public static Tuple<Type, Expression> CreateMemberExpression<T>(this IMemberNameResolver memberNameResolver, ParameterExpression parameter, IEnumerable<string> propertyChain, Type parentType, Expression propertyExpression)
		{
			foreach (var propertyName in propertyChain)
			{
				string name = propertyName;
				var member = memberNameResolver.ResolveAlias(parentType, name);
				if (member != null)
				{
					parentType = GetMemberType(member);
					propertyExpression = propertyExpression == null
											 ? Expression.MakeMemberAccess(parameter, member)
											 : Expression.MakeMemberAccess(propertyExpression, member);
				}
			}

			return new Tuple<Type, Expression>(parentType, propertyExpression);
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

		private static Type GetMemberType(MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;
				case MemberTypes.Property:
					return ((PropertyInfo)member).PropertyType;
				default:
					throw new InvalidOperationException(member.MemberType + " is not resolvable");
			}
		}
	}
}