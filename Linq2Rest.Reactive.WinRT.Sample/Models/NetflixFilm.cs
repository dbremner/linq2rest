using System;
using System.Runtime.Serialization;

namespace Linq2Rest.Reactive.WinRT.Sample.Models
{
	[DataContract]
	public class NetflixFilm
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ReleaseYear { get; set; }

		[DataMember]
		public BoxArt BoxArt { get; set; }
	}

	public class BoxArt
	{
		[DataMember]
		public Uri SmallUrl { get; set; }

		[DataMember]
		public Uri MediumUrl { get; set; }

		[DataMember]
		public Uri LargeUrl { get; set; }

		[DataMember]
		public Uri HighDefinitionUrl { get; set; }
	}
}
