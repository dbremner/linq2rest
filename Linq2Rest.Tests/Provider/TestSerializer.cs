namespace Linq2Rest.Tests.Provider
{
	using System.Collections.Generic;
	using System.Web.Script.Serialization;

	using Linq2Rest.Provider;

	public class TestSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			if (typeof(T).IsAnonymousType())
			{
				return new SimpleAnonymousTypeSerializer<T>();
			}

			return new TestSerializer() as ISerializer<T>;
		}
	}

	public class TestSerializer : ISerializer<SimpleDto>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public SimpleDto Deserialize(string input)
		{
			return _innerSerializer.Deserialize<SimpleDto>(input);
		}

		public IList<SimpleDto> DeserializeList(string input)
		{
			return _innerSerializer.Deserialize<List<SimpleDto>>(input);
		}
	}
}