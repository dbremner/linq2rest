// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

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