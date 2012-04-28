namespace Linq2Rest.Reactive.WP7Sample.Models
{
	using System.Runtime.Serialization;

	[DataContract]
	public class NetflixFilm
	{
		[DataMember]
		public string Name { get; set; }
	}
}
