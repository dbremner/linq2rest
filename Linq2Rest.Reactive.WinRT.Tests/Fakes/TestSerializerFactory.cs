// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using Linq2Rest.Provider;

namespace Linq2Rest.Reactive.WinRT.Tests.Fakes
{
	public class TestSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new TestSerializer<T>();
		}
	}
}