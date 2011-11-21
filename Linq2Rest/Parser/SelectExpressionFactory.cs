// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;

	internal class SelectExpressionFactory<T> : ISelectExpressionFactory<T>
	{
		private readonly IDictionary<string, Func<T, object>> _knownSelections;

		public SelectExpressionFactory()
		{
			_knownSelections = new Dictionary<string, Func<T, object>> { { string.Empty, x => x } };
		}

		public Func<T, object> Create(string selection)
		{
			var fieldNames = (selection ?? string.Empty).Split(',')
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.Trim())
				.OrderBy(x => x);

			var key = string.Join(",", fieldNames);

			if (_knownSelections.ContainsKey(key))
			{
				var knownSelection = _knownSelections[key];

				Contract.Assume(knownSelection != null);

				return knownSelection;
			}

			var elementType = typeof(T);
			var sourceProperties = fieldNames.ToDictionary(name => name, elementType.GetProperty);
			var dynamicType = sourceProperties.Values.GetDynamicType();

			var sourceItem = Expression.Parameter(elementType, "t");
			var bindings =
				dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name])));

			var constructorInfo = dynamicType.GetConstructor(Type.EmptyTypes);

			var selector =
				Expression.Lambda<Func<T, object>>(
					Expression.MemberInit(Expression.New(constructorInfo), bindings), sourceItem)
					.Compile();

			if (Monitor.TryEnter(_knownSelections, 1000))
			{
				_knownSelections.Add(key, selector);

				Monitor.Exit(_knownSelections);
			}

			Contract.Assume(selector != null, "Created above.");

			return selector;
		}
	}
}