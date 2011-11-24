// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;

	using Linq2Rest.Parser;

	public static class ModelFilterExtensions
	{
		public static IEnumerable<object> Filter<T>(this IEnumerable<T> source, ModelFilter<T> filter)
		{
			Contract.Requires<ArgumentNullException>(source != null);

			return filter == null ? source.Cast<object>() : filter.Filter(source);
		}
	}
}