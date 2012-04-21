// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider.Writers
{
	using System;
	using System.IO;

	internal class StreamValueWriter : IValueWriter
	{
		public Type Handles
		{
			get
			{
				return typeof(byte[]);
			}
		}

		public string Write(object value)
		{
			var stream = (Stream)value;
			if (stream.CanSeek)
			{
				stream.Seek(0, SeekOrigin.Begin);
			}

			var buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			var base64 = Convert.ToBase64String(buffer);

			return string.Format("X'{0}'", base64);
		}
	}
}