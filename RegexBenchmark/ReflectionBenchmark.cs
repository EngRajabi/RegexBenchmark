using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace RegexBenchmark
{
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest, BenchmarkDotNet.Order.MethodOrderPolicy.Declared)]
    public class ReflectionBenchmark
    {
        //OriginalCode
        [Benchmark]
        public void OriginalCode()
        {
            var prop = typeof(Test).GetProperties();

            foreach (var item in prop)
            {
                var n = item.Name;
            }
        }

        //UseReflectionHelper
        [Benchmark]
        public void UseReflectionHelper()
        {
            var prop = ReflectionHelper.GetProperties(typeof(Test));

            foreach (var item in prop)
            {
                var n = item.Property.Name;
            }
        }

        ////Case2
        //[Benchmark]
        //public void RegexStaticVariableCase2()
        //{
        //    var f = Regex.IsMatch(Text);
        //}
    }

    public class Test
    {
        public long Id { get; set; }
        public string MyProperty { get; set; }
        public List<string> MyProperty2 { get; set; }
        public List<int> MyProperty3 { get; set; }
        public Test Test2 { get; set; }
    }
}
