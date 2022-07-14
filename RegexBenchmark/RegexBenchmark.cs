using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Text.RegularExpressions;

namespace RegexBenchmark
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class RegexBenchmarkStep2
    {
        private const string Pattern = @"\d+";

        private const string Text =
            "asdjkla j;ajkd;da da asdkswegredsgjvnxvn p ajk;d k;jajkdakjdn k;jbn ak;bjdnak;jbnd 123 ak;bjd ak;sbjd ask;db ;";

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

        //Case4 SourceGenerator
        [Benchmark]
        public void RegexSourceGenerator()
        {
            var f = TestRegexSourceGenerator.IsDigit(Text);
        }
    }

    //TestRegexSourceGenerator
    public static partial class TestRegexSourceGenerator
    {
        // The Source Generator generates the code of the method at compile time
        [RegexGenerator(@"\d+", RegexOptions.Compiled, 50)]
        private static partial Regex IsDigitRegex();

        public static bool IsDigit(string value)
        {
            return IsDigitRegex().IsMatch(value);
        }
    }
}



[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class RegexBenchmarkStep1
{
    private const string Pattern = @"\d+";

    private const string Text =
        "asdjkla j;ajkd;da da asdkswegredsgjvnxvn p ajk;d k;jajkdakjdn k;jbn ak;bjdnak;jbnd 123 ak;bjd ak;sbjd ask;db ;";

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