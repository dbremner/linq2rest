// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelFilterBinder.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the default model binder to bind an <see cref="IModelFilter{T}" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Mvc
{
	using System.Diagnostics.Contracts;
	using System.Web.Mvc;
	using Parser;

	/// <summary>
	/// Defines the default model binder to bind an <see cref="IModelFilter{T}"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ModelFilterBinder<T> : IModelBinder
	{
		private readonly IParameterParser<T> _parser;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelFilterBinder{T}"/> class.
		/// </summary>
		public ModelFilterBinder()
			: this(new ParameterParser<T>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelFilterBinder{T}"/> class.
		/// </summary>
		/// <param name="parser">The <see cref="IParameterParser{T}"/> to use.</param>
		public ModelFilterBinder(IParameterParser<T> parser)
		{
			_parser = parser;
		}

		/// <summary>
		/// Binds the model to a value by using the specified controller context and binding context.
		/// </summary>
		/// <returns>
		/// The bound value.
		/// </returns>
		/// <param name="controllerContext">The controller context.</param><param name="bindingContext">The binding context.</param>
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Contract.Assume(controllerContext != null);
			Contract.Assume(controllerContext.RequestContext != null);
			Contract.Assert(controllerContext.RequestContext.HttpContext != null);
			Contract.Assert(controllerContext.RequestContext.HttpContext.Request != null);
			Contract.Assume(controllerContext.RequestContext.HttpContext.Request.Params != null);

			var request = controllerContext.RequestContext.HttpContext.Request;
			var queryParameters = request.Params;

			return _parser.Parse(queryParameters);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_parser != null);
		}
	}
}