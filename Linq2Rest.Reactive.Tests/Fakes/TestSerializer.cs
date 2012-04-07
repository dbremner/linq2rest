// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using System.Collections.Generic;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;

	public class TestSerializer<T> : ISerializer<T>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public T Deserialize(string input)
		{
			return _innerSerializer.Deserialize<T>(input);
		}

		public IList<T> DeserializeList(string input)
		{
			return _innerSerializer.Deserialize<List<T>>(input ?? "[]");
		}
	}
}