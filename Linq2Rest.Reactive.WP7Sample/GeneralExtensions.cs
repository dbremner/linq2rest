// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest.Reactive.WP7Sample
{
	using System.IO;
	using System.Text;

	internal static class GeneralExtensions
	{
		public static Stream ToStream(this string input)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(input));
		}
	}
}