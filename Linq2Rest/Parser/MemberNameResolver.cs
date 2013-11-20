// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberNameResolver.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the MemberNameResolver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Parser
{
	using System.Collections.Concurrent;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	internal class MemberNameResolver : IMemberNameResolver
	{
		private static readonly ConcurrentDictionary<MemberInfo, string> KnownMemberNames = new ConcurrentDictionary<MemberInfo, string>();

		public string ResolveName(MemberInfo member)
		{
			var result = KnownMemberNames.GetOrAdd(member, ResolveNameInternal);

			Contract.Assume(result != null);

			return result;
		}

		private static string ResolveNameInternal(MemberInfo member)
		{
			Contract.Requires(member != null);

			var dataMember = member.GetCustomAttributes(typeof(DataMemberAttribute), true)
				.OfType<DataMemberAttribute>()
				.FirstOrDefault();

			if (dataMember != null && dataMember.Name != null)
			{
				return dataMember.Name;
			}

			var xmlElement = member.GetCustomAttributes(typeof(XmlElementAttribute), true)
				.OfType<XmlElementAttribute>()
				.FirstOrDefault();

			if (xmlElement != null && xmlElement.ElementName != null)
			{
				return xmlElement.ElementName;
			}

			var xmlAttribute = member.GetCustomAttributes(typeof(XmlAttributeAttribute), true)
				.OfType<XmlAttributeAttribute>()
				.FirstOrDefault();

			if (xmlAttribute != null && xmlAttribute.AttributeName != null)
			{
				return xmlAttribute.AttributeName;
			}

			Contract.Assert(member.Name != null, "Member must have name");
			return member.Name;
		}
	}
}