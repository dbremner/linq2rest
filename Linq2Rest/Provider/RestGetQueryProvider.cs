// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestGetQueryProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestGetQueryProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	internal class RestGetQueryProvider<T> : RestQueryProvider<T>
	{
		public RestGetQueryProvider(IRestClient client, ISerializerFactory serializerFactory, IExpressionProcessor expressionProcessor)
			: base(client, serializerFactory, expressionProcessor)
		{
			Contract.Requires(client != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(expressionProcessor != null);
		}

		protected override IEnumerable<T> GetResults(ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = Client.Get(fullUri);
			var serializer = SerializerFactory.Create<T>();
			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

			return resultSet;
		}

		protected override IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = Client.Get(fullUri);
			var genericMethod = CreateMethod.MakeGenericMethod(type);
			var serializer = genericMethod.Invoke(SerializerFactory, null);
			var deserializeListMethod = serializer.GetType().GetMethod("DeserializeList");
			var resultSet = (IEnumerable)deserializeListMethod.Invoke(serializer, new object[] { response });

			return resultSet;
		}
	}
}