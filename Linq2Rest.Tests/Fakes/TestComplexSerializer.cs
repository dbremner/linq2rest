// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Tests.Fakes
{
	using System.Collections.Generic;
	using System.Web.Script.Serialization;
	using Linq2Rest.Provider;
	using Linq2Rest.Tests.Provider;

	public class TestComplexSerializer : ISerializer<ComplexDto>
	{
		private readonly JavaScriptSerializer _innerSerializer = new JavaScriptSerializer();

		public ComplexDto Deserialize(string input)
		{
			return _innerSerializer.Deserialize<ComplexDto>(input);
		}

		public IList<ComplexDto> DeserializeList(string input)
		{
			return _innerSerializer.Deserialize<List<ComplexDto>>(input);
		}
	}
}