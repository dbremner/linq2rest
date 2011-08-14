namespace UrlQuery.Mvc.Support
{
	/// <summary>
	/// 	Enumeration of supported response formats.
	/// </summary>
	public enum ResponseFormat
	{
		///<summary>
		/// Unknown format.
		///</summary>
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