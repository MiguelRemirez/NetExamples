using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetExamples
{
    [TestClass]
    public class ReflectionExamples
    {

        [TestMethod]
        public void CallExtensionWithReflexionWithGenericArgument()
        {
            List<Test> testList = new List<Test>();

            testList.Add(new Test() { IntProp = 1, StringProp = "3", DateTimeProp = DateTime.Now });
            testList.Add(new Test() { IntProp = 2, StringProp = "2", DateTimeProp = DateTime.Now });
            testList.Add(new Test() { IntProp = 3, StringProp = "1", DateTimeProp = DateTime.Now });

            var query = testList.AsQueryable();
            Expression<Func<Test, int>> orderInt = test => test.IntProp;
            Expression<Func<Test, DateTime>> orderDateTime = test => test.DateTimeProp;
            Expression<Func<Test, string>> orderString = test => test.StringProp;

            Type sourceType = typeof(Test);
            Type keyType = typeof(int);

            var methodtoCall = typeof(HelperExtension).GetMethod(nameof(HelperExtension.ExtensionTest));
            var typedMethod = methodtoCall.MakeGenericMethod(sourceType, keyType);

            var resutl = typedMethod.Invoke(null, new object[] { query, orderInt });


        }


        public class Test
        {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
            public DateTime DateTimeProp { get; set; }
        }

    }
    public static class HelperExtension
    {
        public static IOrderedQueryable<TSource> ExtensionTest<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> where)
        {
            return source.OrderBy(where);
        }

    }
}
