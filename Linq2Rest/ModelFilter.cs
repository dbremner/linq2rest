// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelFilter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ModelFilter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.UI.WebControls;
	using Linq2Rest.Parser;

	internal class ModelFilter<T> : IModelFilter<T>
	{
		private readonly int _skip;
		private readonly int _top;
		private readonly Expression<Func<T, bool>> _filterExpression;
		private readonly Expression<Func<T, object>> _selectExpression;
		private readonly IEnumerable<SortDescription<T>> _sortDescriptions;

		public ModelFilter(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>> selectExpression, IEnumerable<SortDescription<T>> sortDescriptions, int skip, int top)
		{
			_skip = skip;
			_top = top;
			_filterExpression = filterExpression;
			_selectExpression = selectExpression;
			_sortDescriptions = sortDescriptions;
		}

		public IQueryable<object> Filter(IEnumerable<T> model)
		{
			var result = _filterExpression != null
				? model.AsQueryable().Where(_filterExpression)
				: model.AsQueryable();

			if (_sortDescriptions != null && _sortDescriptions.Any())
			{
				var isFirst = true;
				foreach (var sortDescription in _sortDescriptions.Where(x => x != null))
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
						var orderedEnumerable = result as IOrderedQueryable<T>;

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
				Contract.Assume(result != null);

				result = result.Take(_top);
			}

			Contract.Assume(_selectExpression == null || result != null);

			return new UntypedQueryable<T>(result, _selectExpression);
		}
	}
}