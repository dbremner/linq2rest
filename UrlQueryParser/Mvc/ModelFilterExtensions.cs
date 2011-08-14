namespace UrlQueryParser.Mvc
{
	using System.Collections.Generic;

	using UrlQueryParser.Parser;

	public static class ModelFilterExtensions
	{
		public static IEnumerable<object> Filter<T>(this IEnumerable<T> source, ModelFilter<T> filter)
		{
			return filter.Filter(source);
		}
	}
}