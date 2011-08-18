// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Parser
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