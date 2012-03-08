// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using Linq2Rest.Tests.Provider;

namespace Linq2Rest.Tests
{
	using Linq2Rest.Mvc.Provider;
	using Linq2Rest.Provider;

	public class TestSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			if (typeof(T).IsAnonymousType())
			{
				return new RuntimeAnonymousTypeSerializer<T>();
			}

            if (typeof(T) == typeof(SimpleDto)) {
                return new TestSerializer() as ISerializer<T>;
            }
		    return new TestComplexSerializer() as ISerializer<T>;
		}
	}
}