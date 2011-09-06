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
			Contract.Requires<ArgumentNullException>(model != null);

			var result = _filterExpression != null ? model.Where(_filterExpression) : model;

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
						var orderedEnumerable = result as IOrderedEnumerable<T>;

						Contract.Assume(orderedEnumerable != null);

						result = sortDescription.Direction == SortDirection.Ascending
									? orderedEnumerable.ThenBy(sortDescription.KeySelector)
									: orderedEnumerable.ThenByDescending(sortDescription.KeySelector);
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