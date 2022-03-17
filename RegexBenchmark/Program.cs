using System.Text.RegularExpressions;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace RegexBenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //    var t = new RegexBenchmark();
            //    t.RegexStaticMethod();
            //    t.RegexStaticVariable();

            RunBenchmarks();
        }

        private static void RunBenchmarks()
        {
            Regex.CacheSize += 100;
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddAnalyser(BenchmarkDotNet.Analysers.EnvironmentAnalyser.Default)
                .AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub)
                .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default)
                .AddColumn(StatisticColumn.AllStatistics)
                .AddColumn(StatisticColumn.Median)
                .AddColumn(StatisticColumn.StdDev)
                .AddColumn(StatisticColumn.StdErr)
                .AddColumn(StatisticColumn.OperationsPerSecond)
                .AddColumn(BaselineRatioColumn.RatioMean)
                .AddColumn(RankColumn.Arabic)
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60)
                    .WithIterationCount(32)
                    .WithInvocationCount(64)
                    .WithIterationTime(TimeInterval.FromSeconds(120))
                    .WithWarmupCount(6)
                    .WithLaunchCount(1));
            BenchmarkRunner.Run<RegexBenchmarkStep2>(config);
        }
    }
    
}
