namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	internal class RestDeleteQueryProvider<T> : RestQueryProvider<T>
	{
		public RestDeleteQueryProvider(IRestClient client, ISerializerFactory serializerFactory, IExpressionProcessor expressionProcessor)
			: base(client, serializerFactory, expressionProcessor)
		{
		}

		protected override IEnumerable<T> GetResults(ParameterBuilder builder)
		{
			Contract.Requires(builder != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var fullUri = builder.GetFullUri();
			var response = Client.Delete(fullUri);
			var serializer = SerializerFactory.Create<T>();
			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

			return resultSet;
		}

		protected override IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			Contract.Requires(builder != null);

			var fullUri = builder.GetFullUri();
			var response = Client.Delete(fullUri);
			var genericMethod = CreateMethod.MakeGenericMethod(type);
			dynamic serializer = genericMethod.Invoke(SerializerFactory, null);
			var resultSet = serializer.DeserializeList(response);

			return resultSet;
		}
	}
}