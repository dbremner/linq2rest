// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.
// Based on code from http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it

namespace Linq2Rest
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.CompilerServices;
	using System.Runtime.Serialization;
	using System.Threading;

	internal static class LinqExtensions
	{
		private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "Linq2RestTypes" };
		private static readonly ModuleBuilder ModuleBuilder;
		private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();
		private static readonly ConcurrentDictionary<Type, CustomAttributeBuilder[]> TypeAttributeBuilders = new ConcurrentDictionary<Type, CustomAttributeBuilder[]>();
		private static readonly ConcurrentDictionary<MemberInfo, CustomAttributeBuilder[]> PropertyAttributeBuilders = new ConcurrentDictionary<MemberInfo, CustomAttributeBuilder[]>();
		private const MethodAttributes GetSetAttr = MethodAttributes.Final | MethodAttributes.Public;

		static LinqExtensions()
		{
			ModuleBuilder = Thread
				.GetDomain()
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

		public static Type CreateRuntimeType(this Type sourceType, IEnumerable<MemberInfo> properties)
		{
			Contract.Requires<ArgumentNullException>(sourceType != null);
			Contract.Requires<ArgumentNullException>(properties != null);

			properties = properties.ToArray();
			if (!properties.Any())
			{
				throw new ArgumentOutOfRangeException("properties", "properties must have at least 1 property definition");
			}

			var dictionary = properties.ToDictionary(GetMemberAlias, f => f);

			Monitor.Enter(BuiltTypes);

			var className = GetTypeKey(sourceType, dictionary);
			if (BuiltTypes.ContainsKey(className))
			{
				return BuiltTypes[className];
			}

			var typeBuilder = ModuleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);
			SetAttributes(typeBuilder, sourceType);

			foreach (var field in dictionary)
			{
				CreateProperty(typeBuilder, field);
			}

			BuiltTypes[className] = typeBuilder.CreateType();

			Monitor.Exit(BuiltTypes);

			return BuiltTypes[className];
		}

		private static void CreateProperty(TypeBuilder typeBuilder, KeyValuePair<string, MemberInfo> field)
		{
			Contract.Requires(typeBuilder != null);

			var propertyType = field.Value.MemberType == MemberTypes.Property
								? ((PropertyInfo)field.Value).PropertyType
								: ((FieldInfo)field.Value).FieldType;
			var fieldBuilder = typeBuilder.DefineField("_" + field.Key, propertyType, FieldAttributes.Private);

			var propertyBuilder = typeBuilder.DefineProperty(field.Key, PropertyAttributes.None, propertyType, null);

			Contract.Assume(propertyBuilder != null, "Created above.");

			SetAttributes(propertyBuilder, field.Value);

			var getAccessor = typeBuilder.DefineMethod(
														"get_" + field.Key,
														GetSetAttr,
														propertyType,
														Type.EmptyTypes);

			var getIl = getAccessor.GetILGenerator();
			getIl.Emit(OpCodes.Ldarg_0);
			getIl.Emit(OpCodes.Ldfld, fieldBuilder);
			getIl.Emit(OpCodes.Ret);

			var setAccessor = typeBuilder.DefineMethod(
														"set_" + field.Key,
														GetSetAttr,
														null,
														new[] { propertyType });

			var setIl = setAccessor.GetILGenerator();
			setIl.Emit(OpCodes.Ldarg_0);
			setIl.Emit(OpCodes.Ldarg_1);
			setIl.Emit(OpCodes.Stfld, fieldBuilder);
			setIl.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getAccessor);
			propertyBuilder.SetSetMethod(setAccessor);
		}

		private static void SetAttributes(TypeBuilder typeBuilder, Type type)
		{
			var attributeBuilders = TypeAttributeBuilders
				.GetOrAdd(
						  type,
						  t =>
						  {
							  var customAttributes = t.GetCustomAttributesData();
							  return CreateCustomAttributeBuilders(customAttributes).ToArray();
						  });

			Contract.Assume(attributeBuilders != null);

			foreach (var attributeBuilder in attributeBuilders)
			{
				typeBuilder.SetCustomAttribute(attributeBuilder);
			}
		}

		private static void SetAttributes(PropertyBuilder propertyBuilder, MemberInfo memberInfo)
		{
			Contract.Requires(propertyBuilder != null);
			Contract.Requires(memberInfo != null);

			var customAttributeBuilders = PropertyAttributeBuilders
				.GetOrAdd(
						  memberInfo,
						  p =>
						  {
							  var customAttributes = p.GetCustomAttributesData();
							  return CreateCustomAttributeBuilders(customAttributes).ToArray();
						  });

			Contract.Assume(customAttributeBuilders != null);

			foreach (var attribute in customAttributeBuilders)
			{
				propertyBuilder.SetCustomAttribute(attribute);
			}
		}

		private static IEnumerable<CustomAttributeBuilder> CreateCustomAttributeBuilders(IEnumerable<CustomAttributeData> customAttributes)
		{
			Contract.Requires(customAttributes != null);

			var attributeBuilders = customAttributes
				.Select(
						x =>
						{
							var namedArguments = x.NamedArguments;
							var properties = namedArguments.Select(a => a.MemberInfo).OfType<PropertyInfo>().ToArray();
							var values = namedArguments.Select(a => a.TypedValue.Value).ToArray();
							var constructorArgs = x.ConstructorArguments.Select(a => a.Value).ToArray();
							var constructor = x.Constructor;
							return new CustomAttributeBuilder(constructor, constructorArgs, properties, values);
						});
			return attributeBuilders;
		}

		private static string GetMemberAlias(MemberInfo member)
		{
			Contract.Requires(member != null);

			var dataMember = member.GetCustomAttributes(typeof(DataMemberAttribute), true)
				.OfType<DataMemberAttribute>()
				.FirstOrDefault();

			return dataMember == null
					? member.Name
					: dataMember.Name;
		}

		private static string GetTypeKey(Type sourceType, Dictionary<string, MemberInfo> fields)
		{
			Contract.Requires(sourceType != null);
			Contract.Requires(fields != null);

			return fields.Aggregate("Linq2Rest<>" + sourceType.Name, (current, field) => current + (field.Key + field.Value.MemberType));
		}
	}
}