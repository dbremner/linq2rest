// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Parser
{
	using System;
	using System.Collections.Specialized;
	using System.Diagnostics.Contracts;

	public class ParameterParser<T> : IParameterParser<T>
	{
		private readonly IFilterExpressionFactory _filterExpressionFactory;
		private readonly ISortExpressionFactory _sortExpressionFactory;
		private readonly ISelectExpressionFactory<T> _selectExpressionFactory;

		public ParameterParser(
			IFilterExpressionFactory filterExpressionFactory,
			ISortExpressionFactory sortExpressionFactory,
			ISelectExpressionFactory<T> selectExpressionFactory)
		{
			Contract.Requires<ArgumentNullException>(filterExpressionFactory != null);
			Contract.Requires<ArgumentNullException>(sortExpressionFactory != null);
			Contract.Requires<ArgumentNullException>(selectExpressionFactory != null);

			_filterExpressionFactory = filterExpressionFactory;
			_sortExpressionFactory = sortExpressionFactory;
			_selectExpressionFactory = selectExpressionFactory;
		}

		public static IParameterParser<T> Create()
		{
			return new ParameterParser<T>(
				new FilterExpressionFactory(),
				new SortExpressionFactory(),
				new SelectExpressionFactory<T>());
		}

		public ModelFilter<T> Parse(NameValueCollection queryParameters)
		{
			var orderbyField = queryParameters["$orderby"];
			var selects = queryParameters["$select"];
			var filter = queryParameters["$filter"];
			var skip = queryParameters["$skip"];
			var top = queryParameters["$top"];

			var filterExpression = _filterExpressionFactory.Create<T>(filter);
			var sortDescriptions = _sortExpressionFactory.Create<T>(orderbyField);
			var selectFunction = _selectExpressionFactory.Create(selects);

			var modelFilter = new ModelFilter<T>(
				filterExpression.Compile(),
				selectFunction,
				sortDescriptions,
				string.IsNullOrWhiteSpace(skip) ? -1 : Convert.ToInt32(skip),
				string.IsNullOrWhiteSpace(top) ? -1 : Convert.ToInt32(top));
			return modelFilter;
		}
	}
}