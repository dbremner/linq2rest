// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Fakes
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization.Json;
	using System.Web;
	using Linq2Rest.Provider;

	public class InMemoryJsonRestClient<T> : IRestClient
	{
		private readonly IEnumerable<T> _data;
		private readonly DataContractJsonSerializer _serializer;

		public InMemoryJsonRestClient(IEnumerable<T> data, IEnumerable<Type> knownTypes)
		{
			_data = data;
			_serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
		}

		public Uri ServiceBase
		{
			get { return new Uri("http://localhost/InMemoryClient/"); }
		}

		public void Dispose()
		{
		}

		public Stream Get(Uri uri)
		{
			var stream = new MemoryStream();

			_serializer.WriteObject(stream, _data.Filter(HttpUtility.ParseQueryString(uri.Query)).ToArray());
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		public Stream Post(Uri uri, Stream input)
		{
			throw new NotImplementedException();
		}

		public Stream Put(Uri uri, Stream input)
		{
			throw new NotImplementedException();
		}

		public Stream Delete(Uri uri)
		{
			throw new NotImplementedException();
		}
	}
}