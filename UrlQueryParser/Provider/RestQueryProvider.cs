namespace UrlQueryParser.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Web.Script.Serialization;

	public class RestQueryProvider<T> : IQueryProvider
	{
		private readonly IRestClient _client;
		private readonly JavaScriptSerializer _serializer;

		private string _selectParameter;
		private string _filterParameter;
		private readonly List<string> _orderByParameter = new List<string>();
		private string _skipParameter;
		private string _takeParameter;

		public RestQueryProvider(IRestClient client, JavaScriptSerializer serializer)
		{
			_client = client;
			_serializer = serializer;
		}

		public IQueryable CreateQuery(Expression expression) { return new RestQueryable<T>(_client, _serializer, expression); }

		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			return new RestQueryable<TResult>(_client, _serializer, expression);
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
					var selectFunction = methodCall.Arguments[1];
					break;
				case "OrderBy":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]));
					break;
				case "OrderByDescending":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]) + " desc");
					break;
				case "ThenBy":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]));
					break;
				case "ThenByDescending":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]) + " desc");
					break;
				case "Take":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_takeParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Skip":
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_skipParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				default:
					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					var results = GetResults();
					var final = methodCall.Method
						.Invoke(
						null,
						new object[] { results.AsQueryable() }
						.Concat(methodCall.Arguments.Where((x, i) => i > 0).Select(GetExpressionValue)).ToArray());
					return final;
			}

			return null;
		}

		private List<T> GetResults()
		{
			var parameters = string.Format(
				"$filter={0}&$select={1}&$skip={2}&$take={3}&$orderby={4}",
				_filterParameter,
				_selectParameter,
				_skipParameter,
				_takeParameter,
				string.Join(",", _orderByParameter));

			var builder = new UriBuilder(_client.ServiceBase);
			builder.Query = (string.IsNullOrEmpty(builder.Query) ? string.Empty : "&") + parameters;

			var response = _client.GetResponse(builder.Uri);
			var resultSet = _serializer.Deserialize<List<T>>(response);

			return resultSet;
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

		private object GetExpressionValue(Expression expression)
		{
			if(expression is UnaryExpression)
			{
				return (expression as UnaryExpression).Operand;
			}
			if(expression is ConstantExpression)
			{
				return (expression as ConstantExpression).Value;
			}

			return null;
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