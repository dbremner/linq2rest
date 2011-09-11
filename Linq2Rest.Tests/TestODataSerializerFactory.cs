namespace Linq2Rest.Tests
{
	using Linq2Rest.Provider;

	public class TestODataSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new TestODataSerializer<T>();
		}
	}
}