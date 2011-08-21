// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Web;

	internal class RestQueryProvider<T> : IQueryProvider
	{
		private readonly IRestClient _client;
		private readonly ISerializerFactory _serializerFactory;
		private readonly List<string> _orderByParameter = new List<string>();

		private string _selectParameter;
		private string _filterParameter;
		private string _skipParameter;
		private string _takeParameter;

		public RestQueryProvider(IRestClient client, ISerializerFactory serializerFactory)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			Contract.Requires<ArgumentNullException>(serializerFactory != null);

			_client = client;
			_serializerFactory = serializerFactory;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestQueryable<T>(_client, _serializerFactory, expression);
		}

		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return new RestQueryable<TResult>(_client, _serializerFactory, expression);
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
			if (methodCall == null)
			{
				return null;
			}

			var method = methodCall.Method.Name;
			switch (method)
			{
				case "Where":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_filterParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Select":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
					if (unaryExpression != null)
					{
						var lambdaExpression = unaryExpression.Operand as LambdaExpression;
						if (lambdaExpression != null)
						{
							var selectFunction = lambdaExpression.Body as NewExpression;

							if (selectFunction != null)
							{
								var members = selectFunction.Members.Select(x => x.Name).ToArray();
								var args = selectFunction.Arguments.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();
								if (members.Intersect(args).Count() != members.Length)
								{
									throw new InvalidOperationException("Projection into new member names is not supported.");
								}
								_selectParameter = string.Join(",", args);
							}
						}
					}

					break;
				case "OrderBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]));
					break;
				case "OrderByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]) + " desc");
					break;
				case "ThenBy":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]));
					break;
				case "ThenByDescending":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_orderByParameter.Add(ProcessExpression(methodCall.Arguments[1]) + " desc");
					break;
				case "Take":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_takeParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				case "Skip":
					Contract.Assume(methodCall.Arguments.Count >= 2);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					_skipParameter = ProcessExpression(methodCall.Arguments[1]);
					break;
				default:
					Contract.Assume(methodCall.Arguments.Count >= 1);

					ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression);
					var results = GetResults();

					var parameters = new object[] { results.AsQueryable() }
						.Concat(methodCall.Arguments.Where((x, i) => i > 0).Select(GetExpressionValue))
						.ToArray();

					var final = methodCall.Method.Invoke(null, parameters);
					return final;
			}

			return null;
		}

		private IList<T> GetResults()
		{
			Contract.Ensures(Contract.Result<IList<T>>() != null);

			var parameters = new List<string>();
			if (!string.IsNullOrWhiteSpace(_filterParameter))
			{
				parameters.Add("$filter=" + HttpUtility.UrlEncode(_filterParameter));
				// &$select={1}&$skip={2}&$take={3}&$orderby={4}
			}
			if (!string.IsNullOrWhiteSpace(_selectParameter))
			{
				parameters.Add("$select=" + _selectParameter);
			}
			if (!string.IsNullOrWhiteSpace(_skipParameter))
			{
				parameters.Add("$skip=" + _skipParameter);
			}
			if (!string.IsNullOrWhiteSpace(_takeParameter))
			{
				parameters.Add("$take=" + _takeParameter);
			}
			if (_orderByParameter.Any())
			{
				parameters.Add("$orderby=" + string.Join(",", _orderByParameter));
			}

			var builder = new UriBuilder(_client.ServiceBase);
			builder.Query = (string.IsNullOrEmpty(builder.Query) ? string.Empty : "&") + string.Join("&", parameters);
			var response = _client.Get(builder.Uri);

			var serializer = _serializerFactory.Create<T>();

			var resultSet = serializer.DeserializeList(response);

			Contract.Assume(resultSet != null);

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

				return string.Empty;
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

		private string GetMemberCall(MemberExpression memberExpression)
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

		private string GetMethodCall(MethodCallExpression expression)
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

		private object GetExpressionValue(Expression expression)
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

		private string GetOperation(Expression expression)
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

		[ContractInvariantMethod]
		private void Invariants()
		{
			Contract.Invariant(_client != null);
			Contract.Invariant(_serializerFactory != null);
			Contract.Invariant(_orderByParameter != null);
		}
	}
}