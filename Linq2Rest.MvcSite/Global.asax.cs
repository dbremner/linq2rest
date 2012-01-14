// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.MvcSite
{
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	using Linq2Rest.Mvc;
	using Linq2Rest.MvcSite.Models;
	using Linq2Rest.MvcSite.Support;
	using Linq2Rest.Parser;

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
				new { controller = "Home", action = "Index" }); // Parameter defaults
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			var binder = new ModelFilterBinder<SimpleDto>(new ParameterParser<SimpleDto>());

			ModelBinders.Binders.Add(typeof(IModelFilter<SimpleDto>), binder);
			ModelBinders.Binders.Add(typeof(ResponseFormat), new ResponseFormatBinder());
		}
	}
}