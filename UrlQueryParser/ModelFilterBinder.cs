namespace UrlQueryParser
{
	using System.Web.Mvc;

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