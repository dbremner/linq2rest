// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncExpressionProcessor.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   	This source is subject to the Microsoft Public License (Ms-PL).
//   	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   	All other rights reserved.
// </copyright>
// <summary>
//   Defines the IAsyncExpressionProcessor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;
	using Linq2Rest.Provider;

#if !WINDOWS_PHONE
	[ContractClass(typeof(AsyncExpressionProcessorContracts))]
#endif
	internal interface IAsyncExpressionProcessor
	{
		IObservable<T> ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader);
	}

#if !WINDOWS_PHONE
	[ContractClassFor(typeof(IAsyncExpressionProcessor))]
	internal abstract class AsyncExpressionProcessorContracts : IAsyncExpressionProcessor
	{
		public IObservable<T> ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
			Contract.Requires(builder != null);
			Contract.Requires(resultLoader != null);
			Contract.Requires(intermediateResultLoader != null);

			throw new NotImplementedException();
		}
	}
#endif
}