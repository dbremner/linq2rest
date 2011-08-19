// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.Script.Serialization;

	public class SimpleAnonymousTypeSerializer<T> : ISerializer<T>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();
		private readonly Type _elementType = typeof(T);

		public T Deserialize(string input)
		{
			var selectorFunction = CreateSelector(typeof(IDictionary<string, object>));
			var dictionary = _innerSerializer.DeserializeObject(input);

			return selectorFunction(dictionary);
		}

		public IList<T> DeserializeList(string input)
		{
			return ReadToAnonymousType(input);
		}

		private IList<T> ReadToAnonymousType(string response)
		{
			var deserializeObject = _innerSerializer.DeserializeObject(response);
			var enumerable = deserializeObject as IEnumerable;

			if (enumerable == null || !enumerable.OfType<object>().Any())
			{
				return new List<T>();
			}

			var first = enumerable.OfType<object>().First();
			var deserializedType = first.GetType();
			var selectorFunction = CreateSelector(deserializedType);

			Contract.Assume(selectorFunction != null, "Compiled above.");

			return enumerable.OfType<object>().Select<object, T>(selectorFunction).ToList();
		}

		private Func<object, T> CreateSelector(Type deserializedType)
		{
			var objectParameter = Expression.Parameter(typeof(object), "x");
			var fields = _elementType.GetProperties();

			var bindings =
				fields.Select(
					p =>
					Expression.Convert(
						Expression.Call(
							MethodProvider.ChangeTypeMethod,
							Expression.MakeIndex(
								Expression.Convert(objectParameter, deserializedType),
								deserializedType.GetProperty("Item"),
								new[] { Expression.Constant(p.Name) }),
							Expression.Constant(p.PropertyType)),
						p.PropertyType)).ToArray();

			var constructorInfos = _elementType.GetConstructors().ToArray();
			var constructorInfo = constructorInfos.First();

			if (constructorInfo == null)
			{
				throw new NullReferenceException("No public constructor found.");
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