// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.Tests.Fakes
{
	using Linq2Rest.Provider;

	public class TestODataSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new TestODataSerializer<T>();
		}
	}
}