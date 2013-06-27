// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExpressionFactory.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the StreamExpressionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Parser.Readers
{
	using System;
	using System.Diagnostics.CodeAnalysis;
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

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
		public override ConstantExpression Convert(string token)
		{
			var baseResult = base.Convert(token);
			if (baseResult.Value != null)
			{
				var stream = new MemoryStream((byte[])baseResult.Value);

				return Expression.Constant(stream);
			}

			return baseResult;
		}
	}
}