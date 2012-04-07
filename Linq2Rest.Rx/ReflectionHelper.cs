// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive
{
	using System.Diagnostics.Contracts;
	using System.Reflection;
	using Linq2Rest.Provider;

	internal static class ReflectionHelper
	{
		private static readonly MethodInfo InnerCreateMethod = typeof(ISerializerFactory).GetMethod("Create");

		public static MethodInfo CreateMethod
		{
			get
			{
				Contract.Ensures(Contract.Result<MethodInfo>() != null);

				return InnerCreateMethod;
			}
		}
	}
}