namespace UrlQueryParser
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public interface IFilterExpressionFactory
	{
		Expression<Func<T, bool>> Create<T>(string filter);

		Expression<Func<T, bool>> Create<T>(string filter, IFormatProvider formatProvider);
	}

	public interface ISortExpressionFactory
	{
		IEnumerable<SortDescription<T>> Create<T>(string filter);

		IEnumerable<SortDescription<T>> Create<T>(string filter, IFormatProvider formatProvider);
	}
}