using System.Runtime.Serialization;

namespace Linq2Rest.Reactive.WinRT.Sample.Models
{
	[DataContract]
	internal sealed class NetflixFilm
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember(IsRequired = false)]
		public int? ReleaseYear { get; set; }

		[DataMember(IsRequired = false)]
		public double? AverageRating { get; set; }
	}
}
