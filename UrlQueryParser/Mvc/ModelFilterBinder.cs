namespace UrlQueryParser
{
	using System.Web.Mvc;

	public class ModelFilterBinder<T> : IModelBinder
	{
		private readonly IParameterParser _parser;

		public ModelFilterBinder(IParameterParser parser)
		{
			_parser = parser;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var request = controllerContext.RequestContext.HttpContext.Request;
			var queryParameters = request.Params;

			return _parser.Parse<T>(queryParameters);
		}
	}
}