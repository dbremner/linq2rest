// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc.Controllers
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
