namespace UrlQueryParser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.UI.WebControls;

	public class ModelFilter<T>
	{
		private readonly int _skip;

		private readonly int _top;

		private readonly Func<T, bool> _filterExpression;

		private readonly Func<T, object> _selectExpression;

		private readonly IEnumerable<SortDescription<T>> _sortDescriptions;

		public ModelFilter(Func<T, bool> filterExpression, Func<T, object> selectExpression, IEnumerable<SortDescription<T>> sortDescriptions, int skip, int top)
		{
			_skip = skip;
			_top = top;
			_filterExpression = filterExpression;
			_selectExpression = selectExpression;
			_sortDescriptions = sortDescriptions;
		}

		public IEnumerable<object> Filter(IEnumerable<T> model)
		{
			var result = model.Where(_filterExpression);

			if (_sortDescriptions != null && _sortDescriptions.Any())
			{
				var isFirst = true;
				foreach (var sortDescription in _sortDescriptions)
				{
					if (isFirst)
					{
						isFirst = false;
						result = sortDescription.Direction == SortDirection.Ascending
							? result.OrderBy(sortDescription.KeySelector)
							: result.OrderByDescending(sortDescription.KeySelector);
					}
					else
					{
						result = sortDescription.Direction == SortDirection.Ascending
									? (result as IOrderedEnumerable<T>).ThenBy(sortDescription.KeySelector)
									: (result as IOrderedEnumerable<T>).ThenByDescending(sortDescription.KeySelector);
					}
				}
			}

			if (_skip > 0)
			{
				result = result.Skip(_skip);
			}
			if (_top > -1)
			{
				result = result.Take(_top);
			}

			return _selectExpression == null ? result.Cast<object>() : result.Select(_selectExpression);
		}
	}
}