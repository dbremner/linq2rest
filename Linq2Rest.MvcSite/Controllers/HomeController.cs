// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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
