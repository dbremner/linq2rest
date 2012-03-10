// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading;

	internal class ExpressionVisitor : IExpressionVisitor
	{
		private static readonly ExpressionType[] CompositeExpressionTypes = new[] { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };

		public string Visit(Expression expression)
		{
			return expression == null ? null : Visit(expression, expression.Type);
		}

		private static Type GetUnconvertedType(Expression expression)
		{
			Contract.Requires(expression != null);

			switch (expression.NodeType)
			{
				case ExpressionType.Convert:
					var unaryExpression = expression as UnaryExpression;

					Contract.Assume(unaryExpression != null, "Matches node type.");

					return unaryExpression.Operand.Type;
				default:
					return expression.Type;
			}
		}

		private static string GetMemberCall(MemberExpression memberExpression)
		{
			Contract.Requires(memberExpression != null);
			Contract.Ensures(Contract.Result<string>() != null);

			var declaringType = memberExpression.Member.DeclaringType;
			var name = memberExpression.Member.Name;

			if (declaringType == typeof(string))
			{
				if (name == "Length")
				{
					return name.ToLowerInvariant();
				}
			}
			else if (declaringType == typeof(DateTime))
			{
				switch (name)
				{
					case "Hour":
					case "Minute":
					case "Second":
					case "Day":
					case "Month":
					case "Year":
						return name.ToLowerInvariant();
				}
			}

			return string.Empty;
		}

		private static Expression CollapseCapturedOuterVariables(MemberExpression input)
		{
			if (input == null || input.NodeType != ExpressionType.MemberAccess)
			{
				return input;
			}

			if (input.Expression is MemberExpression)
			{
				object value = GetValue(input);
				return Expression.Constant(value);
			}

			var constantExpression = input.Expression as ConstantExpression;
			if (constantExpression != null)
			{
				var obj = constantExpression.Value;
				if (obj == null)
				{
					return input;
				}

				var fieldInfo = input.Member as FieldInfo;
				if (fieldInfo != null)
				{
					var result = fieldInfo.GetValue(obj);
					return result is Expression ? (Expression)result : Expression.Constant(result);
				}

				var propertyInfo = input.Member as PropertyInfo;
				if (propertyInfo != null)
				{
					var result = propertyInfo.GetValue(obj, null);
					return result is Expression ? (Expression)result : Expression.Constant(result);
				}
			}

			return input;
		}

		private static object GetValue(Expression input)
		{
			Contract.Requires(input != null);

			var objectMember = Expression.Convert(input, typeof(object));
			var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

			return getterLambda();
		}

		private static bool IsMemberOfParameter(MemberExpression input)
		{
			Contract.Requires(input != null);

			if (input.Expression == null)
			{
				return false;
			}

			var nodeType = input.Expression.NodeType;
			var tempExpression = input.Expression as MemberExpression;
			while (nodeType == ExpressionType.MemberAccess)
			{
				Contract.Assume(tempExpression != null, "It's a member access");

				nodeType = tempExpression.Expression.NodeType;
				tempExpression = tempExpression.Expression as MemberExpression;
			}

			return nodeType == ExpressionType.Parameter;
		}

		private static string GetOperation(Expression expression)
		{
			Contract.Requires(expression != null);

			switch (expression.NodeType)
			{
				case ExpressionType.Add:
					return "add";
				case ExpressionType.AddChecked:
					break;
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					return "and";
				case ExpressionType.Divide:
					return "div";
				case ExpressionType.Equal:
					return "eq";
				case ExpressionType.GreaterThan:
					return "gt";
				case ExpressionType.GreaterThanOrEqual:
					return "ge";
				case ExpressionType.LessThan:
					return "lt";
				case ExpressionType.LessThanOrEqual:
					return "le";
				case ExpressionType.Modulo:
					return "mod";
				case ExpressionType.Multiply:
					return "mul";
				case ExpressionType.Not:
					return "not";
				case ExpressionType.NotEqual:
					return "ne";
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					return "or";
				case ExpressionType.Subtract:
					return "sub";
			}

			return string.Empty;
		}

		private string GetMethodCall(MethodCallExpression expression)
		{
			Contract.Requires(expression != null);

			var methodName = expression.Method.Name;
			var declaringType = expression.Method.DeclaringType;
			if (declaringType == typeof(string))
			{
				var obj = expression.Object;

				Contract.Assume(obj != null);

				switch (methodName)
				{
					case "Replace":
						{
							Contract.Assume(expression.Arguments.Count > 1);

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];

							Contract.Assume(firstArgument != null);
							Contract.Assume(secondArgument != null);

							return string.Format(
								"replace({0}, {1}, {2})",
								Visit(obj),
								Visit(firstArgument),
								Visit(secondArgument));
						}

					case "Trim":
						return string.Format("trim({0})", Visit(obj));
					case "ToLower":
					case "ToLowerInvariant":
						return string.Format("tolower({0})", Visit(obj));
					case "ToUpper":
					case "ToUpperInvariant":
						return string.Format("toupper({0})", Visit(obj));
					case "Substring":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							if (expression.Arguments.Count == 1)
							{
								var argumentExpression = expression.Arguments[0];

								Contract.Assume(argumentExpression != null);

								return string.Format(
									"substring({0}, {1})", Visit(obj), Visit(argumentExpression));
							}

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];

							Contract.Assume(firstArgument != null);
							Contract.Assume(secondArgument != null);

							return string.Format(
								"substring({0}, {1}, {2})",
								Visit(obj),
								Visit(firstArgument),
								Visit(secondArgument));
						}

					case "IndexOf":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("indexof({0}, {1})", Visit(obj), Visit(argumentExpression));
						}

					case "EndsWith":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("endswith({0}, {1})", Visit(obj), Visit(argumentExpression));
						}

					case "StartsWith":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];

							Contract.Assume(argumentExpression != null);

							return string.Format("startswith({0}, {1})", Visit(obj), Visit(argumentExpression));
						}
				}
			}
			else if (declaringType == typeof(Math))
			{
				Contract.Assume(expression.Arguments.Count > 0);

				var mathArgument = expression.Arguments[0];

				Contract.Assume(mathArgument != null);

				switch (methodName)
				{
					case "Round":
						return string.Format("round({0})", Visit(mathArgument));
					case "Floor":
						return string.Format("floor({0})", Visit(mathArgument));
					case "Ceiling":
						return string.Format("ceiling({0})", Visit(mathArgument));
				}
			}

			if (expression.Method.IsStatic)
			{
				return expression.ToString();
			}

			return string.Empty;
		}

		private string Visit(Expression expression, Type type)
		{
			Contract.Requires(expression != null);

			if (expression is LambdaExpression)
			{
				return Visit((expression as LambdaExpression).Body);
			}

			var memberExpression = expression as MemberExpression;

			if (memberExpression != null)
			{
				var pathPrefixes = new List<string>();
				var currentMemberExpression = memberExpression;
				while (currentMemberExpression != null)
				{
					pathPrefixes.Add(currentMemberExpression.Member.Name);
					currentMemberExpression = currentMemberExpression.Expression as MemberExpression;
				}

				pathPrefixes.Reverse();
				var prefix = string.Join("/", pathPrefixes);

				if (!IsMemberOfParameter(memberExpression))
				{
					var collapsedExpression = CollapseCapturedOuterVariables(memberExpression);
					if (!(collapsedExpression is MemberExpression))
					{
						Contract.Assume(collapsedExpression != null);

						return Visit(collapsedExpression);
					}

					memberExpression = (MemberExpression)collapsedExpression;
				}

				var memberCall = GetMemberCall(memberExpression);

				var innerExpression = memberExpression.Expression;

				Contract.Assume(innerExpression != null);

				return string.IsNullOrWhiteSpace(memberCall)
						? prefix
						: string.Format("{0}({1})", memberCall, Visit(innerExpression));
			}

			if (expression is ConstantExpression)
			{
				var value = (expression as ConstantExpression).Value;

				Contract.Assume(type != null);

				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					"{0}{1}{0}",
					value is string ? "'" : string.Empty,
					value == null ? "null" : GetValue(Expression.Convert(expression, type)));
			}

			if (expression is UnaryExpression)
			{
				var unaryExpression = expression as UnaryExpression;
				var operand = unaryExpression.Operand;
				switch (unaryExpression.NodeType)
				{
					case ExpressionType.Not:
					case ExpressionType.IsFalse:
						return string.Format("not({0})", Visit(operand));
					default:
						return Visit(operand);
				}
			}

			if (expression is BinaryExpression)
			{
				var binaryExpression = expression as BinaryExpression;
				var operation = GetOperation(binaryExpression);

				var isLeftComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Left.NodeType);
				var isRightComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Right.NodeType);

				var leftType = GetUnconvertedType(binaryExpression.Left);
				var leftString = Visit(binaryExpression.Left);
				var rightString = Visit(binaryExpression.Right, leftType);

				return string.Format(
					"{0} {1} {2}",
					string.Format(isLeftComposite ? "({0})" : "{0}", leftString),
					operation,
					string.Format(isRightComposite ? "({0})" : "{0}", rightString));
			}

			if (expression is MethodCallExpression)
			{
				return GetMethodCall(expression as MethodCallExpression);
			}

			if (expression is NewExpression)
			{
				return GetValue(expression).ToString();
			}

			throw new InvalidOperationException("Expression is not recognized or supported");
		}
	}
}