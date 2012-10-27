//  Copyright © Reimers.dk 2012
//	This source is subject to the Microsoft Public License (Ms-PL).
//	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//	All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;

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
		{
			_writer = new ExpressionWriter();
		}

		/// <summary>
		/// Converts an expression into an OData formatted query.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>An OData <see cref="string"/> representation.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Restriction is intended.")]
		public string Convert<T>(Expression<Func<T, bool>> expression)
		{
			return _writer.Write(expression);
		}

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(this._writer != null);
		}
	}
}
