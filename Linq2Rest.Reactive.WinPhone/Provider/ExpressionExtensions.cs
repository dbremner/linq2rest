namespace Linq2Rest.Reactive.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Reflection;

	internal static class ExpressionExtensions
	{
		public static Tuple<Type, string> GetNameFromAlias(this IMemberNameResolver memberNameResolver, MemberInfo alias, Type sourceType)
		{
			Contract.Requires(sourceType != null);
			Contract.Requires(alias != null);
			Contract.Ensures(Contract.Result<Tuple<Type, string>>() != null);

			var source = sourceType.GetMembers()
				.Select(x => new { Original = x, Name = memberNameResolver.ResolveName(x) })
				.FirstOrDefault(x => x.Name == alias.Name);

			return source != null
					   ? new Tuple<Type, string>(GetMemberType(source.Original), source.Name)
					   : new Tuple<Type, string>(GetMemberType(alias), alias.Name);
		}

		private static Type GetMemberType(MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;
				case MemberTypes.Property:
					return ((PropertyInfo)member).PropertyType;
				case MemberTypes.Method:
					return ((MethodInfo)member).ReturnType;
				default:
					throw new InvalidOperationException(member.MemberType + " is not resolvable");
			}
		}
	}
}
