namespace Linq2Rest.Reactive.WP7Sample
{
	using System.Runtime.Serialization;

	[DataContract]
	public class SampleDto
	{
		[DataMember]
		public string Text { get; set; }
	}
}
