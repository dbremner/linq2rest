// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

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