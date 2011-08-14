namespace UrlQueryParser.Mvc
{
	using System.Web.Mvc;

	using UrlQueryParser.Parser;

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