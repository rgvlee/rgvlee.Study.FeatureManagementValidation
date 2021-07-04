using System;
using System.Linq.Expressions;
using Moq;

namespace TestProject1.Extensions
{
    public static class MockExtensions
    {
        public static void Verify<T, TResult>(this T mocked, Expression<Func<T, TResult>> verifyExpression, Func<Times> times) where T : class
        {
            Mock.Get(mocked).Verify(verifyExpression, times);
        }
    }
}