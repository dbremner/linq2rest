// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();
		private readonly Type _elementType = typeof(T);

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

		private IList<T> ReadToAnonymousType(string response)
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

			if (constructorInfo == null)
			{
				throw new InvalidOperationException("No public constructor found.");
			}

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