using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Text.RegularExpressions;

namespace RegexBenchmark
{
    public partial class Program
    {

        static void Main(string[] args)
        {
            RunBenchmarks();
        }

        private static void RunBenchmarks()
        {
            Regex.CacheSize += 100;
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddAnalyser(BenchmarkDotNet.Analysers.EnvironmentAnalyser.Default)
                .AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub)
                .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default)
                .AddColumn(StatisticColumn.OperationsPerSecond);
            //.AddColumn(BaselineRatioColumn.RatioMean)
            //.AddColumn(RankColumn.Arabic);
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.CreateForNewVersion(""))
            //    .WithIterationCount(32)
            //    .WithInvocationCount(64)
            //    .WithIterationTime(TimeInterval.FromSeconds(120))
            //    .WithWarmupCount(6)
            //    .WithLaunchCount(1));

            BenchmarkRunner.Run<RegexBenchmarkStep2>(config);
        }
    }

}
