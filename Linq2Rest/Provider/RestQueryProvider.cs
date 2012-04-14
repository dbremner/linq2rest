// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	internal class RestQueryProvider<T> : RestQueryProviderBase
	{
		private readonly IRestClient _client;
		private readonly ISerializerFactory _serializerFactory;
		private readonly ExpressionProcessor _expressionProcessor;
		private readonly ParameterBuilder _parameterBuilder;

		public RestQueryProvider(IRestClient client, ISerializerFactory serializerFactory)
			: this(client, serializerFactory, new ExpressionProcessor(new ExpressionVisitor()))
		{
		}

		public RestQueryProvider(IRestClient client, ISerializerFactory serializerFactory, ExpressionProcessor expressionProcessor)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_client = client;
			_serializerFactory = serializerFactory;
			_expressionProcessor = expressionProcessor;
			_parameterBuilder = new ParameterBuilder(client.ServiceBase);
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
		public override IQueryable CreateQuery(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestQueryable<T>(_client, _serializerFactory, expression);
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
		public override IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestQueryable<TResult>(_client, _serializerFactory, expression);
		}

		public override object Execute(Expression expression)
		{
			Contract.Assume(expression != null);

			var methodCallExpression = expression as MethodCallExpression;

			return (methodCallExpression != null
					? _expressionProcessor.ProcessMethodCall(methodCallExpression, _parameterBuilder, GetResults, GetIntermediateResults)
					: GetResults(_parameterBuilder))
					?? GetResults(_parameterBuilder);
		}

		public override TResult Execute<TResult>(Expression expression)
		{
			Contract.Assume(expression != null);
			return (TResult)Execute(expression);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client.Dispose();
			}
		}

		private IList<T> GetResults(ParameterBuilder builder)
		{
			Contract.Requires(builder != null);
			Contract.Ensures(Contract.Result<IList<T>>() != null);

			var fullUri = builder.GetFullUri();
			var response = _client.Get(new Uri(fullUri));
			var serializer = _serializerFactory.Create<T>();
			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

			return resultSet;
		}

		private IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			var fullUri = builder.GetFullUri();
			var response = _client.Get(new Uri(fullUri));
			var genericMethod = CreateMethod.MakeGenericMethod(type);
			dynamic serializer = genericMethod.Invoke(_serializerFactory, null);
			var resultSet = serializer.DeserializeList(response);

			return resultSet;
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_client != null);
			Contract.Invariant(_serializerFactory != null);
		}
	}
}