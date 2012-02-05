// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Threading;

	/// <summary>
	/// Defines the SelectExpressionFactory
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of object to project.</typeparam>
	public class SelectExpressionFactory<T> : ISelectExpressionFactory<T>
	{
		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private readonly IDictionary<string, Expression<Func<T, object>>> _knownSelections;

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectExpressionFactory{T}"/> class.
		/// </summary>
		public SelectExpressionFactory()
		{
			_knownSelections = new Dictionary<string, Expression<Func<T, object>>>
			                   	{
			                   		{ string.Empty, null }
			                   	};
		}

		/// <summary>
		/// Creates a select expression.
		/// </summary>
		/// <param name="selection">The properties to select.</param>
		/// <returns>An instance of a <see cref="Func{T1,TResult}"/>.</returns>
		public Expression<Func<T, object>> Create(string selection)
		{
			var fieldNames = (selection ?? string.Empty).Split(',')
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.Trim())
				.OrderBy(x => x);

			var key = string.Join(",", fieldNames);

			if (_knownSelections.ContainsKey(key))
			{
				var knownSelection = _knownSelections[key];

				return knownSelection;
			}

			var elementType = typeof(T);
			var elementMembers = elementType.GetProperties(Flags)
				.Cast<MemberInfo>()
				.Concat(elementType.GetFields(Flags))
				.ToArray();
			var sourceMembers = fieldNames.ToDictionary(name => name, s => elementMembers.First(m => FindMember(s, m)));
			var dynamicType = elementType.CreateRuntimeType(sourceMembers.Values);

			var sourceItem = Expression.Parameter(elementType, "t");
			var bindings = dynamicType
				.GetProperties()
				.Select(p =>
							{
								var member = sourceMembers[p.Name];
								var expression = member.MemberType == MemberTypes.Property
								                 	? Expression.Property(sourceItem, (PropertyInfo)member)
								                 	: Expression.Field(sourceItem, (FieldInfo)member);
								return Expression.Bind(p, expression);
							});

			var constructorInfo = dynamicType.GetConstructor(Type.EmptyTypes);

			Contract.Assume(constructorInfo != null, "Created type has default constructor.");

			var selector = Expression.Lambda<Func<T, object>>(
															  Expression.MemberInit(Expression.New(constructorInfo), bindings),
															  sourceItem);

			if (Monitor.TryEnter(_knownSelections, 1000))
			{
				_knownSelections.Add(key, selector);

				Monitor.Exit(_knownSelections);
			}

			return selector;
		}

		private static bool FindMember(string name, MemberInfo m)
		{
			if (string.Equals(name, m.Name))
			{
				return true;
			}
			var dataMember = m.GetCustomAttributes(typeof(DataMemberAttribute), true)
				.OfType<DataMemberAttribute>()
				.FirstOrDefault();

			if (dataMember != null && string.Equals(name, dataMember.Name))
			{
				return true;
			}

			return false;
		}
	}
}