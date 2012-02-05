// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.CompilerServices;
	using System.Threading;

	internal static class LinqExtensions
	{
		private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicLinqTypes" };
		private static readonly ModuleBuilder ModuleBuilder;
		private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();

		static LinqExtensions()
		{
			ModuleBuilder = Thread.GetDomain()
				.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run)
				.DefineDynamicModule(AssemblyName.Name);
		}

		public static bool IsAnonymousType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
				&& type.IsGenericType
				&& type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
				&& (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
		}

		public static Type GetDynamicType(this IEnumerable<PropertyInfo> fields)
		{
			Contract.Requires<ArgumentNullException>(fields != null);

			if (!fields.Any())
			{
				throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");
			}

			var dictionary = fields.ToDictionary(f => f.Name, f => f.PropertyType);

			Monitor.Enter(BuiltTypes);

			var className = GetTypeKey(dictionary);

			if (BuiltTypes.ContainsKey(className))
			{
				return BuiltTypes[className];
			}

			var typeBuilder = ModuleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

			const MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

			foreach (var field in dictionary)
			{
				var fieldBuilder = typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Private);

				var propertyBuilder = typeBuilder.DefineProperty(field.Key, PropertyAttributes.None, field.Value, null);

				var getAccessor = typeBuilder.DefineMethod(
					"get_" + field.Key,
					GetSetAttr,
					field.Value,
					Type.EmptyTypes);

				var getIl = getAccessor.GetILGenerator();
				getIl.Emit(OpCodes.Ldarg_0);
				getIl.Emit(OpCodes.Ldfld, fieldBuilder);
				getIl.Emit(OpCodes.Ret);

				var setAccessor = typeBuilder.DefineMethod(
					"set_" + field.Key,
					GetSetAttr,
					null,
					new[] { field.Value });

				var setIl = setAccessor.GetILGenerator();
				setIl.Emit(OpCodes.Ldarg_0);
				setIl.Emit(OpCodes.Ldarg_1);
				setIl.Emit(OpCodes.Stfld, fieldBuilder);
				setIl.Emit(OpCodes.Ret);

				propertyBuilder.SetGetMethod(getAccessor);
				propertyBuilder.SetSetMethod(setAccessor);
			}

			BuiltTypes[className] = typeBuilder.CreateType();

			Monitor.Exit(BuiltTypes);

			return BuiltTypes[className];
		}

		private static string GetTypeKey(Dictionary<string, Type> fields)
		{
			return fields.Aggregate("Linq2Rest<>", (current, field) => current + (field.Key + field.Value.Name));
		}
	}
}