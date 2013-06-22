// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the MvcApplication type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.MvcSite
{
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Models;
	using Mvc;
	using Support;

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

			var binder = new ModelFilterBinder<SimpleDto>();

			ModelBinders.Binders.Add(typeof(IModelFilter<SimpleDto>), binder);
			ModelBinders.Binders.Add(typeof(ResponseFormat), new ResponseFormatBinder());
		}
	}
}