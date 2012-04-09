namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System.Runtime.Serialization;

	[DataContract]
	public class SampleDto
	{
		[DataMember]
		public string Text { get; set; }
	}
}
