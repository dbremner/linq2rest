// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
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
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
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
#if !WINDOWS_PHONE && !NETFX_CORE
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
#endif

                return InnerCreateMethod;
			}
		}
	}
}