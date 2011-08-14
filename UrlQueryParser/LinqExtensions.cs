namespace UrlQueryParser.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Threading;

	public static class LinqExtensions
	{
		private static readonly AssemblyName AssemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
		private static readonly ModuleBuilder ModuleBuilder;
		private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();

		static LinqExtensions()
		{
			ModuleBuilder = Thread.GetDomain().DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
		}

		private static string GetTypeKey(Dictionary<string, Type> fields)
		{
			//TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter

			return fields.Aggregate(string.Empty, (current, field) => current + (field.Key + ";" + field.Value.Name + ";"));
		}

		private static Type GetDynamicType(this Dictionary<string, Type> fields)
		{
			if (null == fields)
			{
				throw new ArgumentNullException("fields");
			}
			if (0 == fields.Count)
			{
				throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");
			}

			try
			{
				Monitor.Enter(BuiltTypes);
				var className = GetTypeKey(fields);

				if (BuiltTypes.ContainsKey(className))
				{
					return BuiltTypes[className];
				}

				var typeBuilder = ModuleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

				foreach (var field in fields)
					typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

				BuiltTypes[className] = typeBuilder.CreateType();

				return BuiltTypes[className];
			}
			catch
			{
				return null;
			}
			finally
			{
				Monitor.Exit(BuiltTypes);
			}
		}
		
		private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
		{
			return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
		}

		public static Type GetDynamicType(this IEnumerable<PropertyInfo> fields)
		{
			return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
		}
	}
}