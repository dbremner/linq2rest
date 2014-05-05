// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataExpressionConverter.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Converts LINQ expressions to OData queries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using Linq2Rest.Provider.Writers;

	/// <summary>
	/// Converts LINQ expressions to OData queries.
	/// </summary>
	public class ODataExpressionConverter
	{
		private readonly IExpressionWriter _writer;

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataExpressionConverter"/> class.
		/// </summary>
		public ODataExpressionConverter()
			: this(new IntValueWriter[0])
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataExpressionConverter"/> class.
		/// </summary>
		/// <param name="valueWriters">The custom value writers to use.</param>
		/// <param name="memberNameResolver">The custom <see cref="IMemberNameResolver"/> to use.</param>
		public ODataExpressionConverter(IEnumerable<IValueWriter> valueWriters, IMemberNameResolver memberNameResolver = null)
		{
			_writer = new ExpressionWriter(memberNameResolver ?? new MemberNameResolver(), valueWriters);
		}

		/// <summary>
		/// Converts an expression into an OData formatted query.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>An OData <see cref="string"/> representation.</returns>
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Restriction is intended.")]
		public string Convert<T>(Expression<Func<T, bool>> expression)
		{
			return _writer.Write(expression, typeof(T));
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_writer != null);
		}
	}
}
