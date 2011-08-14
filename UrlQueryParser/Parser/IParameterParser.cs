namespace UrlQueryParser.Parser
{
	using System.Collections.Specialized;

	using UrlQueryParser.Mvc;

	public interface IParameterParser
	{
		ModelFilter<T> Parse<T>(NameValueCollection queryParameters);
	}
}