namespace Linq2Rest.Reactive.SL.IntegrationTests
{
	using System.IO;
	using System.Text;

	public static class TestExtensions
	{
		public static Stream ToStream(this string input)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(input));
		}
	}
}