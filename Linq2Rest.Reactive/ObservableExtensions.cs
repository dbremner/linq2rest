// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq;
	using System.Reactive.Linq;
	using System.Reflection;

	internal static class InteralObservableExtensions
	{
		private static readonly MethodInfo _innerToObservableMethod;
		private static readonly MethodInfo _innerToQbservableMethod;

		static InteralObservableExtensions()
		{
#if !NETFX_CORE
			var qbservableMethods = typeof(Qbservable).GetMethods(BindingFlags.Static | BindingFlags.Public);
			var observableMethods = typeof(Observable).GetMethods(BindingFlags.Static | BindingFlags.Public);
#else
			var qbservableMethods = typeof(Qbservable).GetTypeInfo().GetDeclaredMethods("AsQbservable").ToArray();
			var observableMethods = typeof(Observable).GetTypeInfo().GetDeclaredMethods("ToObservable").ToArray();
#endif

#if !WINDOWS_PHONE
			Contract.Assume(qbservableMethods.Length > 0);
			Contract.Assume(observableMethods.Length > 0);
#endif

			_innerToObservableMethod = observableMethods
					.First(x => x.Name == "ToObservable" && x.GetParameters().Length == 1);
			_innerToQbservableMethod = qbservableMethods
					.First(x => x.Name == "AsQbservable" && x.GetParameters().Length == 1);
		}

		public static object ToQbservable(this IEnumerable enumerable, Type type)
		{
#if !WINDOWS_PHONE
			Contract.Requires(enumerable != null);
			Contract.Requires(type != null);
#endif

			var genericObservableMethod = _innerToObservableMethod.MakeGenericMethod(type);
			var genericQbservableMethod = _innerToQbservableMethod.MakeGenericMethod(type);

			var observable = genericObservableMethod.Invoke(null, new object[] { enumerable });
			var qbservable = genericQbservableMethod.Invoke(null, new[] { observable });

			return qbservable;
		}
	}
}