// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

using System.Linq;
using System.Linq.Expressions;
using Linq2Rest.Provider;

namespace Linq2Rest
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	internal static class LinqExtensions
	{
		public static bool IsAnonymousType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
				&& type.IsGenericType
				&& type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
				&& (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
		}



	}
}