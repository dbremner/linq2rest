// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuntimeAnonymousTypeSerializer.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
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
	using System.Reflection;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;

	/// <summary>
	/// Serializes simples annymous type structures.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> to serialize.</typeparam>
	/// <typeparam name="TSource">The <see cref="Type"/> to load alias data from.</typeparam>
	public class RuntimeAnonymousTypeSerializer<T, TSource> : ISerializer<T>
	{
		private readonly IMemberNameResolver _nameResolver;
		private readonly Type _sourceType = typeof(TSource);
		private readonly Type _elementType = typeof(T);
		private readonly Type _deserializedType = typeof(Dictionary<string, object>);
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		/// <summary>
		/// a
		/// </summary>
		/// <param name="nameResolver">The <see cref="IMemberNameResolver"/> used to resolve name aliasing.</param>
		public RuntimeAnonymousTypeSerializer(IMemberNameResolver nameResolver)
		{
			_nameResolver = nameResolver;
		}

		/// <summary>
		/// Deserializes a single item.
		/// </summary>
		/// <param name="input">The serialized item.</param>
		/// <returns>An instance of the serialized item.</returns>
		public T Deserialize(Stream input)
		{
			var content = new StreamReader(input).ReadToEnd();

			var dictionary = (Dictionary<string, object>)_innerSerializer.DeserializeObject(content);
			var selectorFunction = CreateSelector(dictionary);
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

		private static Type GetMemberType(MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;
				case MemberTypes.Method:
					return ((MethodInfo)member).ReturnType;
				case MemberTypes.Property:
					return ((PropertyInfo)member).PropertyType;
				default:
					throw new ArgumentOutOfRangeException("member", "Cannot handle " + member.MemberType);
			}
		}

		private IEnumerable<T> ReadToAnonymousType(string response)
		{
			var deserializeObject = _innerSerializer.DeserializeObject(response);
			var enumerable = deserializeObject as IEnumerable;

			if (enumerable == null)
			{
				return new List<T>();
			}

			var objectEnumerable = enumerable.OfType<Dictionary<string, object>>().ToArray();

			if (objectEnumerable.Length == 0)
			{
				return new List<T>();
			}

			var first = objectEnumerable[0];

			var selectorFunction = CreateSelector(first);

			Contract.Assume(selectorFunction != null, "Compiled above.");

			return objectEnumerable.Select(selectorFunction).ToList();
		}

		private Func<object, T> CreateSelector(IDictionary<string, object> deserializedObject)
		{
			var objectParameter = Expression.Parameter(typeof(object), "x");
			var keys = deserializedObject.Keys;

			var properties = typeof(T).GetProperties();

			var bindings = (from key in keys
							let alias = _nameResolver.ResolveAlias(_sourceType, key)
							where properties.Any(p => p.Name == alias.Name)
							let member = _nameResolver.ResolveName(alias)
							let memberType = GetMemberType(alias)
							let arguments = new[] { Expression.Constant(member) }
							let indexExpression = Expression.MakeIndex(
								Expression.Convert(objectParameter, _deserializedType),
								_deserializedType.GetProperty("Item"),
								arguments)
							select Expression.Convert(
								Expression.Call(
									AnonymousTypeSerializerHelper.InnerChangeTypeMethod,
									indexExpression,
									Expression.Constant(memberType)),
								memberType))
				.ToArray();

			var constructorInfo = _elementType.GetConstructors().FirstOrDefault();

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