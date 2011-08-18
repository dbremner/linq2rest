// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc
{
	using System.Web.Mvc;

	using Linq2Rest.Parser;

	public class ModelFilterBinder<T> : IModelBinder
	{
		private readonly IParameterParser<T> _parser;

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