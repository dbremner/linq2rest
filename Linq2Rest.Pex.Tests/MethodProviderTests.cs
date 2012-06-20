// <copyright file="MethodProviderTests.cs" company="Reimers.dk">Copyright © Reimers.dk 2011</copyright>
using System;
using System.Linq.Expressions;
using System.Reflection;
using Linq2Rest;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Linq2Rest
{
    /// <summary>This class contains parameterized unit tests for MethodProvider</summary>
    [PexClass(typeof(MethodProvider))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class MethodProviderTests
    {
        /// <summary>Test stub for get_ContainsMethod()</summary>
        [PexMethod]
        internal MethodInfo ContainsMethodGet()
        {
            MethodInfo result = MethodProvider.ContainsMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.ContainsMethodGet()
        }

        /// <summary>Test stub for get_DayProperty()</summary>
        [PexMethod]
        internal PropertyInfo DayPropertyGet()
        {
            PropertyInfo result = MethodProvider.DayProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.DayPropertyGet()
        }

        /// <summary>Test stub for get_DecimalCeilingMethod()</summary>
        [PexMethod]
        internal MethodInfo DecimalCeilingMethodGet()
        {
            MethodInfo result = MethodProvider.DecimalCeilingMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DecimalCeilingMethodGet()
        }

        /// <summary>Test stub for get_DecimalFloorMethod()</summary>
        [PexMethod]
        internal MethodInfo DecimalFloorMethodGet()
        {
            MethodInfo result = MethodProvider.DecimalFloorMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DecimalFloorMethodGet()
        }

        /// <summary>Test stub for get_DecimalRoundMethod()</summary>
        [PexMethod]
        internal MethodInfo DecimalRoundMethodGet()
        {
            MethodInfo result = MethodProvider.DecimalRoundMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DecimalRoundMethodGet()
        }

        /// <summary>Test stub for get_DoubleCeilingMethod()</summary>
        [PexMethod]
        internal MethodInfo DoubleCeilingMethodGet()
        {
            MethodInfo result = MethodProvider.DoubleCeilingMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DoubleCeilingMethodGet()
        }

        /// <summary>Test stub for get_DoubleFloorMethod()</summary>
        [PexMethod]
        internal MethodInfo DoubleFloorMethodGet()
        {
            MethodInfo result = MethodProvider.DoubleFloorMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DoubleFloorMethodGet()
        }

        /// <summary>Test stub for get_DoubleRoundMethod()</summary>
        [PexMethod]
        internal MethodInfo DoubleRoundMethodGet()
        {
            MethodInfo result = MethodProvider.DoubleRoundMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.DoubleRoundMethodGet()
        }

        /// <summary>Test stub for get_EndsWithMethod()</summary>
        [PexMethod]
        internal MethodInfo EndsWithMethodGet()
        {
            MethodInfo result = MethodProvider.EndsWithMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.EndsWithMethodGet()
        }

        /// <summary>Test stub for GetAnyAllMethod(String, Type)</summary>
        [PexMethod]
        internal MethodInfo GetAnyAllMethod(string name, Type collectionType)
        {
            MethodInfo result = MethodProvider.GetAnyAllMethod(name, collectionType);
            return result;
            // TODO: add assertions to method MethodProviderTests.GetAnyAllMethod(String, Type)
        }

        /// <summary>Test stub for GetIEnumerableImpl(Type)</summary>
        [PexMethod]
        internal Type GetIEnumerableImpl(Type type)
        {
            Type result = MethodProvider.GetIEnumerableImpl(type);
            return result;
            // TODO: add assertions to method MethodProviderTests.GetIEnumerableImpl(Type)
        }

        /// <summary>Test stub for get_HourProperty()</summary>
        [PexMethod]
        internal PropertyInfo HourPropertyGet()
        {
            PropertyInfo result = MethodProvider.HourProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.HourPropertyGet()
        }

        /// <summary>Test stub for get_IgnoreCaseExpression()</summary>
        [PexMethod]
        internal ConstantExpression IgnoreCaseExpressionGet()
        {
            ConstantExpression result = MethodProvider.IgnoreCaseExpression;
            return result;
            // TODO: add assertions to method MethodProviderTests.IgnoreCaseExpressionGet()
        }

        /// <summary>Test stub for get_IndexOfMethod()</summary>
        [PexMethod]
        internal MethodInfo IndexOfMethodGet()
        {
            MethodInfo result = MethodProvider.IndexOfMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.IndexOfMethodGet()
        }

        /// <summary>Test stub for get_LengthProperty()</summary>
        [PexMethod]
        internal PropertyInfo LengthPropertyGet()
        {
            PropertyInfo result = MethodProvider.LengthProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.LengthPropertyGet()
        }

        /// <summary>Test stub for get_MinuteProperty()</summary>
        [PexMethod]
        internal PropertyInfo MinutePropertyGet()
        {
            PropertyInfo result = MethodProvider.MinuteProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.MinutePropertyGet()
        }

        /// <summary>Test stub for get_MonthProperty()</summary>
        [PexMethod]
        internal PropertyInfo MonthPropertyGet()
        {
            PropertyInfo result = MethodProvider.MonthProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.MonthPropertyGet()
        }

        /// <summary>Test stub for get_SecondProperty()</summary>
        [PexMethod]
        internal PropertyInfo SecondPropertyGet()
        {
            PropertyInfo result = MethodProvider.SecondProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.SecondPropertyGet()
        }

        /// <summary>Test stub for get_StartsWithMethod()</summary>
        [PexMethod]
        internal MethodInfo StartsWithMethodGet()
        {
            MethodInfo result = MethodProvider.StartsWithMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.StartsWithMethodGet()
        }

        /// <summary>Test stub for get_SubstringMethod()</summary>
        [PexMethod]
        internal MethodInfo SubstringMethodGet()
        {
            MethodInfo result = MethodProvider.SubstringMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.SubstringMethodGet()
        }

        /// <summary>Test stub for get_ToLowerMethod()</summary>
        [PexMethod]
        internal MethodInfo ToLowerMethodGet()
        {
            MethodInfo result = MethodProvider.ToLowerMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.ToLowerMethodGet()
        }

        /// <summary>Test stub for get_ToUpperMethod()</summary>
        [PexMethod]
        internal MethodInfo ToUpperMethodGet()
        {
            MethodInfo result = MethodProvider.ToUpperMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.ToUpperMethodGet()
        }

        /// <summary>Test stub for get_TrimMethod()</summary>
        [PexMethod]
        internal MethodInfo TrimMethodGet()
        {
            MethodInfo result = MethodProvider.TrimMethod;
            return result;
            // TODO: add assertions to method MethodProviderTests.TrimMethodGet()
        }

        /// <summary>Test stub for get_YearProperty()</summary>
        [PexMethod]
        internal PropertyInfo YearPropertyGet()
        {
            PropertyInfo result = MethodProvider.YearProperty;
            return result;
            // TODO: add assertions to method MethodProviderTests.YearPropertyGet()
        }
    }
}
