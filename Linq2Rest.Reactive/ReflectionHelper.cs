// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ReflectionHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System.Diagnostics.Contracts;

	using System.Reflection;
	using Linq2Rest.Provider;

	internal static class ReflectionHelper
	{
#if !NETFX_CORE
		private static readonly MethodInfo InnerCreateMethod = typeof(ISerializerFactory).GetMethod("Create");
#else
		private static readonly MethodInfo InnerCreateMethod =
	        typeof(ISerializerFactory)
            .GetTypeInfo()
            .GetDeclaredMethod("Create");
#endif

		public static MethodInfo CreateMethod
		{
			get
            {
#if !NETFX_CORE
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
#endif

                return InnerCreateMethod;
			}
		}
	}
}