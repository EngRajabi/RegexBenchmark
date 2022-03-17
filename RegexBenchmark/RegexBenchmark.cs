using System;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace RegexBenchmark
{
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest, BenchmarkDotNet.Order.MethodOrderPolicy.Declared)]
    public class RegexBenchmarkStep1
    {
        private const string Pattern = @"\d+";
        private const string Text = "asdjkla j;ajkd;da da asdkswegredsgjvnxvn p ajk;d k;jajkdakjdn k;jbn ak;bjdnak;jbnd 123 ak;bjd ak;sbjd ask;db ;";

        private static readonly Regex Regex = new(Pattern);

        //OriginalCode
        [Benchmark]
        public void OriginalCode()
        {
            var regex = new Regex(Pattern);
            var f = regex.IsMatch(Text);
        }

        //Case1
        [Benchmark]
        public void RegexStaticMethodCase1()
        {
            var f = Regex.IsMatch(Text, Pattern);
        }

        //Case2
        [Benchmark]
        public void RegexStaticVariableCase2()
        {
            var f = Regex.IsMatch(Text);
        }
    }

    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest, BenchmarkDotNet.Order.MethodOrderPolicy.Declared)]
    public class RegexBenchmarkStep2
    {
        private const string Pattern = @"\d+";
        private const string Text = "asdjkla j;ajkd;da da asdkswegredsgjvnxvn p ajk;d k;jajkdakjdn k;jbn ak;bjdnak;jbnd 123 ak;bjd ak;sbjd ask;db ;";

        private static readonly RegexOptions regexOptions = RegexOptions.Compiled;
        private static readonly Regex Regex = new(Pattern, regexOptions, TimeSpan.FromMilliseconds(50));

        //OriginalCode
        [Benchmark]
        public void OriginalCode()
        {
            var regex = new Regex(Pattern, regexOptions, TimeSpan.FromMilliseconds(50));
            var f = regex.IsMatch(Text);
        }

        //Case1
        [Benchmark]
        public void RegexStaticMethodCase1()
        {
            var f = Regex.IsMatch(Text, Pattern, regexOptions, TimeSpan.FromMilliseconds(50));
        }

        //Case2
        [Benchmark]
        public void RegexStaticVariableCase2()
        {
            var f = Regex.IsMatch(Text);
        }
    }
}
