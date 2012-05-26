// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using Linq2Rest.Provider;

namespace Linq2Rest.Reactive.WinRT.Sample.Support
{
	public class ODataSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			return new ODataSerializer<T>();
		}
	}
}