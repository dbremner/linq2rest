// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc.Support
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	public class ResponseFormatBinder : IModelBinder
	{
		readonly HeaderParser _headerParser = new HeaderParser();

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{

			var preferred = _headerParser.GetPreferredContentType(
				controllerContext.HttpContext.Request,
				ResponseFormatExtensions.SupportedFormats());

			var format = preferred.ToResponseFormat();

			return format;
		}

		private class HeaderParser
		{
			/// <summary>
			/// Gets the preferred content type for the response.
			/// </summary>
			/// <param name="request">The HTTP request.</param>
			/// <param name="supportedContentTypes">The supported response types.</param>
			/// <returns>The preferred supported content type.</returns>
			public string GetPreferredContentType(HttpRequestBase request, IEnumerable<string> supportedContentTypes)
			{
				Contract.Requires(supportedContentTypes != null);

				return request == null
					|| request.AcceptTypes == null
						? null
						: request.AcceptTypes
						.Where(x => !string.IsNullOrWhiteSpace(x))
						.OrderBy(GetMimeTypePriority)
						.Select(CleanContentType)
						.Intersect(supportedContentTypes)
						.FirstOrDefault();
			}

			private static double GetMimeTypePriority(string mimeType)
			{
				var mimeQuality = mimeType.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				if (mimeQuality.Length < 2)
				{
					return 1d;
				}

				var qualityString = mimeQuality[1];
				var quality = qualityString.Substring(qualityString.IndexOf('=') + 1).Trim();
				return Convert.ToDouble(quality);
			}

			private string CleanContentType(string contentType)
			{
				var result = string.IsNullOrEmpty(contentType) || contentType.IndexOf(';') < 0
						? contentType
						: contentType.Substring(0, contentType.IndexOf(';')).Trim();
				return result;
			}
		}
	}
}