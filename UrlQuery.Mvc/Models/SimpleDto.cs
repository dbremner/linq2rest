namespace UrlQuery.Mvc.Models
{
	using System.ComponentModel.DataAnnotations;

	public class SimpleDto
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public string Content { get; set; }

		public double Value { get; set; }
	}
}