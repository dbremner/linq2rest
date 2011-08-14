﻿namespace UrlQuery.Mvc.Controllers
{
	using System.Web.Mvc;
	using System;
	using UrlQuery.Mvc.Models;
	using UrlQuery.Mvc.Support;
	using UrlQueryParser.Mvc;
	using UrlQueryParser.Parser;

	public class SimpleController : Controller
	{
		private readonly SimpleContext _db = new SimpleContext();

		public ActionResult Index(ModelFilter<SimpleDto> filter, ResponseFormat format)
		{
			var model = _db.SimpleDtos
				.Filter(filter);

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