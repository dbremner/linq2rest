// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestPutQueryProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestPutQueryProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;

	internal class RestPutQueryProvider<T> : RestQueryProvider<T>
	{
		private readonly Stream _inputData;

		public RestPutQueryProvider(IRestClient client, ISerializerFactory serializerFactory, IExpressionProcessor expressionProcessor, Stream inputData)
			: base(client, serializerFactory, expressionProcessor)
		{
			Contract.Requires(client != null);
			Contract.Requires(serializerFactory != null);
			Contract.Requires(expressionProcessor != null);
			Contract.Requires(inputData != null);

			_inputData = inputData;
		}

		protected override IEnumerable<T> GetResults(ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = Client.Put(fullUri, _inputData);
			var serializer = SerializerFactory.Create<T>();
			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

			return resultSet;
		}

		protected override IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = Client.Put(fullUri, _inputData);
			var genericMethod = CreateMethod.MakeGenericMethod(type);
			dynamic serializer = genericMethod.Invoke(SerializerFactory, null);
			var resultSet = serializer.DeserializeList(response);

			return resultSet;
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_inputData != null);
		}
	}
}