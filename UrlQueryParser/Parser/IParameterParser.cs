// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace UrlQueryParser.Parser
{
	using System.Collections.Specialized;

	public interface IParameterParser<T>
	{
		ModelFilter<T> Parse(NameValueCollection queryParameters);
	}
}