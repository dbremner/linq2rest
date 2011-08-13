namespace UrlQueryParser.Provider
{
	using System.Linq;

	public class RestContext<T>
	{
		public RestContext() { }

		public IQueryable Query
		{
			get
			{
				return new RestQueryable<T>();
			}
		}
	}
}