// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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
		private static readonly MethodInfo _innerCreateMethod = typeof(ISerializerFactory).GetMethod("Create");
#else
	    private static readonly MethodInfo InnerCreateMethod =
	        typeof (ISerializerFactory)
            .GetTypeInfo()
            .GetDeclaredMethod("Create");
#endif

		public static MethodInfo CreateMethod
		{
			get
			{
#if !WINDOWS_PHONE
				Contract.Ensures(Contract.Result<MethodInfo>() != null);
#endif

				return _innerCreateMethod;
			}
		}
	}
}