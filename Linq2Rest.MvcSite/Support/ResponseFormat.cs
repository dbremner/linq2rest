// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseFormat.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2011
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Enumeration of supported response formats.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.MvcSite.Support
{
	/// <summary>
	/// 	Enumeration of supported response formats.
	/// </summary>
	public enum ResponseFormat
	{
		/// <summary>
		///		Unknown format.
		/// </summary>
		Unknown,

		/// <summary>
		/// 	Plain text response.
		/// </summary>
		Txt,

		/// <summary>
		/// 	JSON response.
		/// </summary>
		JS,

		/// <summary>
		/// 	XML response.
		/// </summary>
		XML,

		/// <summary>
		/// 	KML response.
		/// </summary>
		KML,

		/// <summary>
		/// 	HTML response.
		/// </summary>
		HTML,

		/// <summary>
		/// 	PNG image response.
		/// </summary>
		Png,

		/// <summary>
		/// 	GIF image response.
		/// </summary>
		Gif
	}
}