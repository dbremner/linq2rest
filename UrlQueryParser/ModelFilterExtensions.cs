namespace UrlQueryParser
{
	using System.Collections.Generic;

	public static class ModelFilterExtensions
	{
		public static IEnumerable<object> Filter<T>(this IEnumerable<T> source, ModelFilter<T> filter)
		{
			return filter.Filter(source);
		}
	}
}