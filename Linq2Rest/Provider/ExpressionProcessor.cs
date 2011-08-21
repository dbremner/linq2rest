namespace Linq2Rest.Provider
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq.Expressions;
	using System.Threading;

	internal static class ExpressionProcessor
	{
		public static string ProcessExpression(this Expression expression)
		{
			if (expression is LambdaExpression)
			{
				return ProcessExpression((expression as LambdaExpression).Body);
			}
			if (expression is MemberExpression)
			{
				var memberExpression = expression as MemberExpression;
				var memberCall = GetMemberCall(memberExpression);

				return string.IsNullOrWhiteSpace(memberCall)
				       	? memberExpression.Member.Name
				       	: string.Format("{0}({1})", memberCall, ProcessExpression(memberExpression.Expression));
			}
			if (expression is ConstantExpression)
			{
				var value = (expression as ConstantExpression).Value;
				return string.Format
					(Thread.CurrentThread.CurrentCulture,
						"{0}{1}{0}",
						value is string ? "'" : string.Empty,
						value);
			}
			if (expression is UnaryExpression)
			{
				var unaryExpression = expression as UnaryExpression;
				var operand = unaryExpression.Operand;
				switch (unaryExpression.NodeType)
				{
					case ExpressionType.Not:
						return string.Format("not({0})", ProcessExpression(operand));
					case ExpressionType.Quote:
						return ProcessExpression(operand);
				}
			}
			if (expression is BinaryExpression)
			{
				var binaryExpression = expression as BinaryExpression;
				var operation = GetOperation(binaryExpression);

				return string.Format
					("{0} {1} {2}", ProcessExpression(binaryExpression.Left), operation, ProcessExpression(binaryExpression.Right));
			}
			if (expression is MethodCallExpression)
			{
				return GetMethodCall(expression as MethodCallExpression);
			}

			return string.Empty;
		}

		private static string GetMemberCall(MemberExpression memberExpression)
		{
			Contract.Requires(memberExpression != null);

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

		private static string GetMethodCall(MethodCallExpression expression)
		{
			Contract.Requires(expression != null);

			var methodName = expression.Method.Name;
			var declaringType = expression.Method.DeclaringType;
			if (declaringType == typeof(string))
			{
				switch (methodName)
				{
					case "Replace":
						{
							Contract.Assume(expression.Arguments.Count > 1);

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];

							return string.Format(
								"replace({0}, {1}, {2})",
								ProcessExpression(expression.Object),
								ProcessExpression(firstArgument),
								ProcessExpression(secondArgument));
						}
					case "Trim":
						return string.Format("trim({0})", ProcessExpression(expression.Object));
					case "ToLower":
					case "ToLowerInvariant":
						return string.Format("tolower({0})", ProcessExpression(expression.Object));
					case "ToUpper":
					case "ToUpperInvariant":
						return string.Format("toupper({0})", ProcessExpression(expression.Object));
					case "Substring":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							if (expression.Arguments.Count == 1)
							{
								var argumentExpression = expression.Arguments[0];
								return string.Format(
									"substring({0}, {1})", ProcessExpression(expression.Object), ProcessExpression(argumentExpression));
							}

							var firstArgument = expression.Arguments[0];
							var secondArgument = expression.Arguments[1];
							return string.Format(
								"substring({0}, {1}, {2})",
								ProcessExpression(expression.Object),
								ProcessExpression(firstArgument),
								ProcessExpression(secondArgument));
						}
					case "IndexOf":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];
							return string.Format(
								"indexof({0}, {1})", ProcessExpression(expression.Object), ProcessExpression(argumentExpression));
						}
					case "EndsWith":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];
							return string.Format(
								"endswith({0}, {1})", ProcessExpression(expression.Object), ProcessExpression(argumentExpression));
						}
					case "StartsWith":
						{
							Contract.Assume(expression.Arguments.Count > 0);

							var argumentExpression = expression.Arguments[0];
							return string.Format(
								"startswith({0}, {1})", ProcessExpression(expression.Object), ProcessExpression(argumentExpression));
						}
				}
			}
			else if (declaringType == typeof(Math))
			{
				Contract.Assume(expression.Arguments.Count > 0);

				var mathArgument = expression.Arguments[0];

				switch (methodName)
				{
					case "Round":
						return string.Format("round({0})", ProcessExpression(mathArgument));
					case "Floor":
						return string.Format("floor({0})", ProcessExpression(mathArgument));
					case "Ceiling":
						return string.Format("ceiling({0})", ProcessExpression(mathArgument));
				}
			}

			return string.Empty;
		}

		public static object GetExpressionValue(this Expression expression)
		{
			if (expression is UnaryExpression)
			{
				return (expression as UnaryExpression).Operand;
			}
			if (expression is ConstantExpression)
			{
				return (expression as ConstantExpression).Value;
			}

			return null;
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

	}
}