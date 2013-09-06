﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleController.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.MvcSite.Controllers
{
	using System;
	using System.Linq;
	using System.Web.Mvc;
	using Linq2Rest.MvcSite.Models;
	using Linq2Rest.MvcSite.Support;

	public class SimpleController : Controller
	{
		private readonly SimpleContext _db = new SimpleContext();

		public ActionResult Index(IModelFilter<SimpleDto> filter, ResponseFormat format)
		{
			var model = _db.SimpleDtos.Filter(filter);
			var count = model.Count();
			
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