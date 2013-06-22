// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSerializerFactory.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TestSerializerFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Fakes
{
	using Linq2Rest.Mvc.Provider;
	using Linq2Rest.Provider;
	using Provider;

	public class TestSerializerFactory : ISerializerFactory
	{
		public ISerializer<T> Create<T>()
		{
			if (typeof(T).IsAnonymousType())
			{
				return new RuntimeAnonymousTypeSerializer<T>();
			}

			if (typeof(T) == typeof(ComplexDto))
			{
				return new TestComplexSerializer() as ISerializer<T>;
			}

			return new TestSerializer<T>();
		}
	}
}