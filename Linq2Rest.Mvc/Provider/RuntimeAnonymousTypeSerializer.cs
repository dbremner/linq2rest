// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuntimeAnonymousTypeSerializer.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Serializes simples annymous type structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Mvc.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;

	/// <summary>
	/// Serializes simples annymous type structures.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> to serialize.</typeparam>
	public class RuntimeAnonymousTypeSerializer<T> : ISerializer<T>
	{
		private readonly Type _elementType = typeof(T);
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		/// <summary>
		/// Deserializes a single item.
		/// </summary>
		/// <param name="input">The serialized item.</param>
		/// <returns>An instance of the serialized item.</returns>
		public T Deserialize(Stream input)
		{
			var content = new StreamReader(input).ReadToEnd();
			var selectorFunction = CreateSelector(typeof(IDictionary<string, object>));
			var dictionary = _innerSerializer.DeserializeObject(content);

			return selectorFunction(dictionary);
		}

		/// <summary>
		/// Deserializes a list of items.
		/// </summary>
		/// <param name="input">The serialized items.</param>
		/// <returns>An list of the serialized items.</returns>
		public IEnumerable<T> DeserializeList(Stream input)
		{
			var content = new StreamReader(input).ReadToEnd();
			return ReadToAnonymousType(content);
		}

		/// <summary>
		/// Serializes the passed item into a <see cref="Stream"/>.
		/// </summary>
		/// <param name="item">The item to serialize.</param>
		/// <returns>A <see cref="Stream"/> representation of the item.</returns>
		public Stream Serialize(T item)
		{
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			writer.Write(_innerSerializer.Serialize(item));
			writer.Flush();
			ms.Position = 0;
			return ms;
		}

		private IEnumerable<T> ReadToAnonymousType(string response)
		{
			var deserializeObject = _innerSerializer.DeserializeObject(response);
			var enumerable = deserializeObject as IEnumerable;

			if (enumerable == null)
			{
				return new List<T>();
			}

			var objectEnumerable = enumerable.OfType<object>().ToArray();

			if (objectEnumerable.Length == 0)
			{
				return new List<T>();
			}

			var first = objectEnumerable[0];
			var deserializedType = first.GetType();
			var selectorFunction = CreateSelector(deserializedType);

			Contract.Assume(selectorFunction != null, "Compiled above.");

			return objectEnumerable.Select(selectorFunction).ToList();
		}

		private Func<object, T> CreateSelector(Type deserializedType)
		{
			var objectParameter = Expression.Parameter(typeof(object), "x");
			var fields = _elementType.GetProperties();

			var bindings = fields
				.Select(
						p =>
						{
							var arguments = new[] { Expression.Constant(p.Name) };

							var indexExpression = Expression.MakeIndex(
										   Expression.Convert(objectParameter, deserializedType),
										   deserializedType.GetProperty("Item"),
										   arguments);

							return Expression.Convert(
										 Expression.Call(
														 AnonymousTypeSerializerHelper.InnerChangeTypeMethod,
														 indexExpression,
														 Expression.Constant(p.PropertyType)),
										 p.PropertyType);
						})
				.ToArray();

			var constructorInfos = _elementType.GetConstructors().ToArray();
			var constructorInfo = constructorInfos.FirstOrDefault();

			var selector =
				Expression.Lambda<Func<object, T>>(
					Expression.New(constructorInfo, bindings), objectParameter);
			var selectorFunction = selector.Compile();

			return selectorFunction;
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_innerSerializer != null);
			Contract.Invariant(_elementType != null);
		}
	}
}