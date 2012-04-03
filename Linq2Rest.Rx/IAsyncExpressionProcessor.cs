// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	internal interface IAsyncExpressionProcessor
	{
		Task<IList<T>> ProcessMethodCall<T>(
			MethodCallExpression methodCall,
			ReactiveParameterBuilder builder,
			Func<ReactiveParameterBuilder, Task<IList<T>>> resultLoader,
			Func<Type, ReactiveParameterBuilder, Task<IEnumerable>> intermediateResultLoader);
	}
}