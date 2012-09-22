// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestDeleteQueryProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestDeleteQueryProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
			var fullUri = builder.GetFullUri();
			var response = Client.Delete(fullUri);
			var serializer = SerializerFactory.Create<T>();
			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

			return resultSet;
		}

		protected override IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = Client.Delete(fullUri);
			var genericMethod = CreateMethod.MakeGenericMethod(type);
			dynamic serializer = genericMethod.Invoke(SerializerFactory, null);
			var resultSet = serializer.DeserializeList(response);

			return resultSet;
		}
	}
}