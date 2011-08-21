namespace Linq2Rest.Tests
{
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
}