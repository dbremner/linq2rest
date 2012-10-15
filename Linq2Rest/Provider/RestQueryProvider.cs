// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestQueryProvider.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RestQueryProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	[ContractClass(typeof(RestQueryProviderContracts<>))]
	internal abstract class RestQueryProvider<T> : RestQueryProviderBase
	{
		private readonly IExpressionProcessor _expressionProcessor;
		private readonly ParameterBuilder _parameterBuilder;

		public RestQueryProvider(IRestClient client, ISerializerFactory serializerFactory, IExpressionProcessor expressionProcessor)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);
			Contract.Requires<ArgumentNullException>(expressionProcessor != null);

			Client = client;
			SerializerFactory = serializerFactory;
			_expressionProcessor = expressionProcessor;
			_parameterBuilder = new ParameterBuilder(client.ServiceBase);
		}

		protected IRestClient Client { get; private set; }
		
		protected ISerializerFactory SerializerFactory { get; private set; }

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
		public override IQueryable CreateQuery(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestGetQueryable<T>(Client, SerializerFactory, expression);
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
		public override IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestGetQueryable<TResult>(Client, SerializerFactory, expression);
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
				Client.Dispose();
			}
		}

		protected abstract IEnumerable<T> GetResults(ParameterBuilder builder);

		protected abstract IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder);

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(Client != null);
			Contract.Invariant(SerializerFactory != null);
			Contract.Invariant(_expressionProcessor != null);
			Contract.Invariant(_parameterBuilder != null);
		}
	}

	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Keeping contracts with class.")]
	[ContractClassFor(typeof(RestQueryProvider<>))]
	internal abstract class RestQueryProviderContracts<T> : RestQueryProvider<T>
	{
		protected RestQueryProviderContracts(IRestClient client, ISerializerFactory serializerFactory, IExpressionProcessor expressionProcessor)
			: base(client, serializerFactory, expressionProcessor)
		{
		}

		protected override IEnumerable<T> GetResults(ParameterBuilder builder)
		{
			Contract.Requires(builder != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			throw new NotImplementedException();
		}

		protected override IEnumerable GetIntermediateResults(Type type, ParameterBuilder builder)
		{
			Contract.Requires(builder != null);

			throw new NotImplementedException();
		}
	}
}