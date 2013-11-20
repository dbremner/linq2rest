// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.MvcSite.Controllers
{
	using System.Web.Mvc;

	public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
