namespace UrlQueryParser
{
	using System.Collections.Specialized;

	public interface IParameterParser
	{
		ModelFilter<T> Parse<T>(NameValueCollection queryParameters);
	}
}