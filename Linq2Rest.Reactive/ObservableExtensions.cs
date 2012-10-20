// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableExtensions.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ObservableExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Linq2Rest.Provider;

namespace Linq2Rest.Reactive
{
	using System;

	public static class ObservableExtensions
	{
		public static IObservable<T> Post<T, TInput>(this IObservable<T> source, TInput input)
		{
			var restObservable = source as InnerRestObservableBase<T>;
			if (restObservable != null)
			{
				restObservable.ChangeMethod(HttpMethod.Post);
				var serializer = restObservable.SerializerFactory.Create<TInput>();
				var serialized = serializer.Serialize(input);
				restObservable.SetInput(serialized);
			}

			return source;
		}
	}
}