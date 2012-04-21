// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.IO;
	using System.Linq.Expressions;

	internal class StreamExpressionFactory : ByteArrayExpressionFactory
	{
		public override Type Handles
		{
			get
			{
				return typeof(Stream);
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public override ConstantExpression Convert(string token)
		{
			var baseResult = base.Convert(token);

			var stream = new MemoryStream((byte[])baseResult.Value);

			return Expression.Constant(stream);
		}
	}
}