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
			return new MemoryStream(Encoding.UTF8.GetBytes(input));
		}
	}
}