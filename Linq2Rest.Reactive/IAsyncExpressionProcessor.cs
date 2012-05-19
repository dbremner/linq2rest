// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if !WINDOWS_PHONE
	using System.Diagnostics.Contracts;
#endif
	using System.Linq.Expressions;
	using System.Threading.Tasks;
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
#endif
	internal abstract class AsyncExpressionProcessorContracts : IAsyncExpressionProcessor
	{
		public IObservable<T> ProcessMethodCall<T>(MethodCallExpression methodCall, ParameterBuilder builder, Func<ParameterBuilder, IObservable<IEnumerable<T>>> resultLoader, Func<Type, ParameterBuilder, IObservable<IEnumerable>> intermediateResultLoader)
		{
#if !WINDOWS_PHONE
			Contract.Requires<ArgumentNullException>(resultLoader != null);
#endif

			throw new NotImplementedException();
		}
	}
}