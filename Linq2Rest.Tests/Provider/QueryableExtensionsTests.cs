﻿namespace Linq2Rest.Tests.Provider
{
	using System.Linq;
	using System.Threading;
	using Linq2Rest.Provider;
	using NUnit.Framework;

	[TestFixture]
	public class QueryableExtensionsTests
	{
		[Test]
		public void WhenExecutingQueryAsynchronouslyThenDoesNotExecuteOnTestThread()
		{
			var testThreadId = Thread.CurrentThread.ManagedThreadId;
			var source = new[] { 1, 2, 3, 4, 5 };
			var queryableTask = source
				.AsQueryable()
				.Select(x => new { ThreadId = Thread.CurrentThread.ManagedThreadId })
				.ExecuteAsync();

			queryableTask.Wait();

			var queryableResult = queryableTask.Result.First();
		
			Assert.AreNotEqual(testThreadId, queryableResult.ThreadId);
		}
	}
}
