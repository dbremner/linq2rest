// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedIntExpressionFactoryTests.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the UnsignedIntExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Linq2Rest.Tests.Parser.Readers
{
	using System;
	using Linq2Rest.Parser.Readers;
	using NUnit.Framework;

	[TestFixture]
	public class UnsignedIntExpressionFactoryTests
	{
		[SetUp]
		public void Setup()
		{
			_factory = new UnsignedIntExpressionFactory();
		}

		private UnsignedIntExpressionFactory _factory;

		[Test]
		public void WhenFilterIncludesUnsignedIntParameterThenReturnedExpressionContainsUnsignedInt()
		{
			var expression = _factory.Convert("123");

			Assert.IsAssignableFrom<uint>(expression.Value);
		}

		[Test]
		public void WhenFilterIsIncorrectFormatThenThrows()
		{
			const string Parameter = "blah";

			Assert.Throws<FormatException>(() => _factory.Convert(Parameter));
		}
	}
}