// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableExtensions.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ObservableExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;

	/// <summary>
	/// Defines public extension methods on <see cref="IObservable{T}"/>
	/// </summary>
	public static class ObservableExtensions
	{
		/// <summary>
		/// Creates an <see cref="IObservable{T}"/> over a POST request.
		/// </summary>
		/// <param name="source">The source <see cref="IObservable{T}"/>.</param>
		/// <param name="input">A <see cref="Func{TInput}"/> to generate POST data.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item in the <see cref="IObservable{T}"/>.</typeparam>
		/// <typeparam name="TInput">The <see cref="Type"/> of item to POST to the server.</typeparam>
		/// <returns>An <see cref="IObservable{T}"/> instance.</returns>
		public static IObservable<T> Post<T, TInput>(this IObservable<T> source, Func<TInput> input)
		{
			var restObservable = source as InnerRestObservableBase<T>;
			if (restObservable != null)
			{
				restObservable.ChangeMethod(HttpMethod.Post);
				var serializer = restObservable.SerializerFactory.Create<TInput>();
				var serialized = serializer.Serialize(input());
				restObservable.SetInput(serialized);
			}

			return source;
		}

		/// <summary>
		/// Creates an <see cref="IObservable{T}"/> over a PUT request.
		/// </summary>
		/// <param name="source">The source <see cref="IObservable{T}"/>.</param>
		/// <param name="input">A <see cref="Func{TInput}"/> to generate PUT data.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item in the <see cref="IObservable{T}"/>.</typeparam>
		/// <typeparam name="TInput">The <see cref="Type"/> of item to PUT on the server.</typeparam>
		/// <returns>An <see cref="IObservable{T}"/> instance.</returns>
		public static IObservable<T> Put<T, TInput>(this IObservable<T> source, Func<TInput> input)
		{
			var restObservable = source as InnerRestObservableBase<T>;
			if (restObservable != null)
			{
				restObservable.ChangeMethod(HttpMethod.Put);
				var serializer = restObservable.SerializerFactory.Create<TInput>();
				var serialized = serializer.Serialize(input());
				restObservable.SetInput(serialized);
			}

			return source;
		}

		/// <summary>
		/// Creates an <see cref="IObservable{T}"/> over a DELETE request.
		/// </summary>
		/// <param name="source">The source <see cref="IObservable{T}"/>.</param>
		/// <typeparam name="T">The <see cref="Type"/> of item in the <see cref="IObservable{T}"/>.</typeparam>
		/// <returns>An <see cref="IObservable{T}"/> instance.</returns>
		public static IObservable<T> Delete<T>(this IObservable<T> source)
		{
			var restObservable = source as InnerRestObservableBase<T>;
			if (restObservable != null)
			{
				restObservable.ChangeMethod(HttpMethod.Delete);
			}

			return source;
		}
	}
}