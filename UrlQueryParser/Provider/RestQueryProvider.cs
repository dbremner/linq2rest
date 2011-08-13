namespace UrlQueryParser.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;

	public class RestQueryProvider<T> : IQueryProvider
	{
		private readonly Uri _serviceBase;

		private string _selectParameter;
		private string _filterParameter;
		private string _skipParameter;
		private string _takeParameter;

		public RestQueryProvider(Uri serviceBase)
		{
			_serviceBase = serviceBase;
		}

		public IQueryable CreateQuery(Expression expression) { return new RestQueryable<T>(_serviceBase, expression); }

		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			return new RestQueryable<TResult>(_serviceBase, expression);
		}

		public object Execute(Expression expression)
		{
			return (expression is MethodCallExpression
					? ProcessMethodCall(expression as MethodCallExpression)
					: ProcessExpression(expression))
					?? GetResults();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return (TResult)Execute(expression);
		}

		private object ProcessMethodCall(MethodCallExpression methodCall)
		{
			var method = methodCall.Method.Name;
			switch (method)
			{
				case "Where":
					_filterParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Select":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_selectParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Take":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_takeParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Skip":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_skipParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Count":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					return GetResults().Count();
			}

			return null;
		}

		private IEnumerable<T> GetResults()
		{
			var parameters = string.Format("$filter={0}&$select={1}&$skip={2}&$take={3}", _filterParameter, _selectParameter, _skipParameter, _takeParameter);

			var builder = new UriBuilder(_serviceBase);
			builder.Query = (string.IsNullOrEmpty(builder.Query) ? string.Empty : "&") + parameters;
			var resultSet = new[] { builder.Uri.AbsoluteUri, _filterParameter, _selectParameter };

			return resultSet.OfType<T>();
		}

		private string ProcessExpression(Expression expression)
		{
			if (expression is LambdaExpression)
			{
				return ProcessExpression((expression as LambdaExpression).Body);
			}
			if (expression is MemberExpression)
			{
				return (expression as MemberExpression).Member.Name;
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
					case ExpressionType.Quote:
						return ProcessExpression(operand);
				}

				return string.Empty;
			}
			if (expression is BinaryExpression)
			{
				var binaryExpression = expression as BinaryExpression;
				var operation = GetOperation(binaryExpression);

				return string.Format
					("{0} {1} {2}", ProcessExpression(binaryExpression.Left), operation, ProcessExpression(binaryExpression.Right));
			}
			return string.Empty;
		}

		private string GetOperation(Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Add:
					return "add";
				case ExpressionType.AddChecked:
					break;
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					return "and";
				case ExpressionType.Call:
					break;
				case ExpressionType.Conditional:
					break;
				case ExpressionType.Convert:
					break;
				case ExpressionType.ConvertChecked:
					break;
				case ExpressionType.Divide:
					return "div";
				case ExpressionType.Equal:
					return "eq";
				case ExpressionType.ExclusiveOr:
					break;
				case ExpressionType.GreaterThan:
					return "gt";
				case ExpressionType.GreaterThanOrEqual:
					return "ge";
				case ExpressionType.Invoke:
					break;
				case ExpressionType.LeftShift:
					break;
				case ExpressionType.LessThan:
					break;
				case ExpressionType.LessThanOrEqual:
					return "le";
				case ExpressionType.MemberAccess:
					break;
				case ExpressionType.MemberInit:
					break;
				case ExpressionType.Modulo:
					return "mod";
				case ExpressionType.Multiply:
					return "mul";
				case ExpressionType.MultiplyChecked:
					break;
				case ExpressionType.Negate:
					break;
				case ExpressionType.UnaryPlus:
					break;
				case ExpressionType.NegateChecked:
					break;
				case ExpressionType.Not:
					return "not";
				case ExpressionType.NotEqual:
					return "ne";
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					return "or";
				case ExpressionType.Power:
					break;
				case ExpressionType.RightShift:
					break;
				case ExpressionType.Subtract:
					return "sub";
				case ExpressionType.SubtractChecked:
					break;
				case ExpressionType.TypeAs:
					break;
				case ExpressionType.TypeIs:
					break;
				case ExpressionType.Assign:
					break;
				case ExpressionType.Block:
					break;
				case ExpressionType.DebugInfo:
					break;
				case ExpressionType.Decrement:
					break;
				case ExpressionType.Dynamic:
					break;
				case ExpressionType.Default:
					break;
				case ExpressionType.Increment:
					break;
				case ExpressionType.Index:
					break;
				case ExpressionType.AddAssign:
					break;
				case ExpressionType.AndAssign:
					break;
				case ExpressionType.DivideAssign:
					break;
				case ExpressionType.ExclusiveOrAssign:
					break;
				case ExpressionType.LeftShiftAssign:
					break;
				case ExpressionType.ModuloAssign:
					break;
				case ExpressionType.MultiplyAssign:
					break;
				case ExpressionType.OrAssign:
					break;
				case ExpressionType.PowerAssign:
					break;
				case ExpressionType.RightShiftAssign:
					break;
				case ExpressionType.SubtractAssign:
					break;
				case ExpressionType.AddAssignChecked:
					break;
				case ExpressionType.MultiplyAssignChecked:
					break;
				case ExpressionType.SubtractAssignChecked:
					break;
				case ExpressionType.PreIncrementAssign:
					break;
				case ExpressionType.PreDecrementAssign:
					break;
				case ExpressionType.PostIncrementAssign:
					break;
				case ExpressionType.PostDecrementAssign:
					break;
				case ExpressionType.TypeEqual:
					break;
				case ExpressionType.OnesComplement:
					break;
				case ExpressionType.IsTrue:
					break;
				case ExpressionType.IsFalse:
					break;
			}

			return string.Empty;
		}
	}
}