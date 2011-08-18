using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlQuery.Mvc
{
	using UrlQuery.Mvc.Models;
	using UrlQuery.Mvc.Support;
	using UrlQueryParser.Mvc;
	using UrlQueryParser.Parser;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}", // URL with parameters
				new { controller = "Home", action = "Index" } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			var binder = new ModelFilterBinder<SimpleDto>(ParameterParser<SimpleDto>.Create());

			ModelBinders.Binders.Add(typeof(ModelFilter<SimpleDto>), binder);
			ModelBinders.Binders.Add(typeof(ResponseFormat), new ResponseFormatBinder());
		}
	}
}