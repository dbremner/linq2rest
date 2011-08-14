namespace UrlQueryParser.Parser
{
	using System.Collections.Specialized;

	public interface IParameterParser<T>
	{
		ModelFilter<T> Parse(NameValueCollection queryParameters);
	}
}