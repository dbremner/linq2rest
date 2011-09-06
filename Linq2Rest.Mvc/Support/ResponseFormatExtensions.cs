// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Mvc.Support
{
	using System.Collections.Generic;

	public static class ResponseFormatExtensions
	{
		public static IEnumerable<string> SupportedFormats()
		{
			return new[]
				{
					"text/javascript",
					"application/x-javascript",
					"application/javascript",
					"text/ecmascript",
					"application/ecmascript",
					"text/jscript",
					"application/xhtml+xml",
					"text/html",
					"text/plain"
				};
		}

		public static ResponseFormat ToResponseFormat(this string acceptFormat)
		{
			if (acceptFormat == null)
			{
				return ResponseFormat.Unknown;
			}

			switch (acceptFormat)
			{
				case "text/javascript":
				case "application/x-javascript":
				case "application/javascript":
				case "text/ecmascript":
				case "application/ecmascript":
				case "text/jscript":
					return ResponseFormat.JS;
				case "application/xhtml+xml":
				case "text/html":
					return ResponseFormat.HTML;
				case "image/png":
					return ResponseFormat.Png;
				case "text/plain":
					return ResponseFormat.Txt;
					////case "application/xml":
					////case "text/xml":
					////    return ResponseFormat.XML;
				default:
					break;
			}

			return ResponseFormat.Unknown;
		}

		public static string ToContentType(this ResponseFormat format)
		{
			switch (format)
			{
				case ResponseFormat.JS:
					return "text/javascript";
				case ResponseFormat.HTML:
					return "text/html";
					////case ResponseFormat.XML:
					////    return "text/xml";
				default:
					return "text/plain";
			}
		}
	}
}