namespace UrlQueryParser
{
	using System;
	using System.Collections.Specialized;

	public class ParameterParser : IParameterParser
	{
		private readonly IFilterExpressionFactory _filterExpressionFactory;
		private readonly ISortExpressionFactory _sortExpressionFactory;

		public ParameterParser(IFilterExpressionFactory filterExpressionFactory, ISortExpressionFactory sortExpressionFactory)
		{
			_filterExpressionFactory = filterExpressionFactory;
			_sortExpressionFactory = sortExpressionFactory;
		}

		public ModelFilter<T> Parse<T>(NameValueCollection queryParameters)
		{
			var orderbyField = queryParameters["$orderby"];

			var filter = queryParameters["$filter"];
			var skip = queryParameters["$skip"];
			var top = queryParameters["$top"];
			var filterExpression = _filterExpressionFactory.Create<T>(filter);
			var sortDescriptions = _sortExpressionFactory.Create<T>(orderbyField);

			var modelFilter = new ModelFilter<T>
				(
				filterExpression.Compile(),
				null,
				sortDescriptions,
				string.IsNullOrWhiteSpace(skip) ? -1 : Convert.ToInt32(skip),
				string.IsNullOrWhiteSpace(top) ? -1 : Convert.ToInt32(top));
			return modelFilter;
		}
	}
}