//  Copyright © Reimers.dk 2011
//	This source is subject to the Microsoft Public License (Ms-PL).
//	Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//	All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// Converts LINQ expressions to OData queries.
	/// </summary>
	public class ODataExpressionConverter
	{
		private readonly IExpressionVisitor _visitor;

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataExpressionConverter"/> class.
		/// </summary>
		public ODataExpressionConverter()
		{
			_visitor = new ExpressionVisitor();
		}

		/// <summary>
		/// Converts an expression into an OData formatted query.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>An OData <see cref="string"/> representation.</returns>
		public string Convert<T>(Expression<Func<T, bool>> expression)
		{
			return _visitor.Visit(expression);
		}
	}
}
