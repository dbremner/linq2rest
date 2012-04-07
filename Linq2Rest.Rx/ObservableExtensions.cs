// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Linq;
	using System.Reactive.Linq;
	using System.Reflection;

	internal static class InteralObservableExtensions
	{
		private static readonly MethodInfo InnerToObservableMethod =
			typeof(Observable)
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x => x.Name == "ToObservable" && x.GetParameters().Length == 1);

		private static readonly MethodInfo InnerToQbservableMethod =
			typeof(Qbservable)
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x => x.Name == "AsQbservable" && x.GetParameters().Length == 1);

		public static object ToQbservable(this IEnumerable enumerable, Type type)
		{
			var genericObservableMethod = InnerToObservableMethod.MakeGenericMethod(type);
			var genericQbservableMethod = InnerToQbservableMethod.MakeGenericMethod(type);

			var observable = genericObservableMethod.Invoke(null, new object[] { enumerable });
			var qbservable = genericQbservableMethod.Invoke(null, new object[] { observable });

			return qbservable;
		}
	}
}