// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.MvcSite.Controllers
{
	using System;
	using System.Web.Mvc;

	using Linq2Rest.MvcSite.Models;
	using Linq2Rest.MvcSite.Support;

	public class SimpleController : Controller
	{
		private readonly SimpleContext _db = new SimpleContext();

		public ActionResult Index(IModelFilter<SimpleDto> filter, ResponseFormat format)
		{
			var model = _db.SimpleDtos.Filter(filter);

			switch (format)
			{
				case ResponseFormat.JS:
					return Json(model, JsonRequestBehavior.AllowGet);
				case ResponseFormat.HTML:
					return View(model);
				default:
					throw new ArgumentOutOfRangeException("format");
			}
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}