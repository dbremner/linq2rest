namespace UrlQueryParser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;

	public interface ISelectExpressionFactory<T>
	{
		Func<T, object> Create(string selection);
	}

	public class SelectExpressionFactory<T> : ISelectExpressionFactory<T>
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
				return _knownSelections[key];
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

			return selector;
		}
	}
}