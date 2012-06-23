// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest.Mvc.Provider
{
	using System;
	using System.Reflection;

	internal static class AnonymousTypeSerializerHelper
	{
		public static readonly MethodInfo InnerChangeTypeMethod = typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });
	}
}