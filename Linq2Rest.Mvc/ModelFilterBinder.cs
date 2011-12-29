// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc
{
	using System.Web.Mvc;
	using Linq2Rest.Parser;

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
		/// <param name="parser">The <see cref="IParameterParser{T}"/> to use.</param>
		public ModelFilterBinder(IParameterParser<T> parser)
		{
			_parser = parser;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var request = controllerContext.RequestContext.HttpContext.Request;
			var queryParameters = request.Params;

			return _parser.Parse(queryParameters);
		}
	}
}