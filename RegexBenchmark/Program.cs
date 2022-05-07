using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Perfolizer.Horology;

namespace RegexBenchmark
{
    //public static partial class Log
    //{
    //    [LoggerMessage(
    //        EventId = 0,
    //        Message = "Could not open socket to {LoggerObjRecord}")]
    //    public static partial void LogHelloWorld(ILogger logger,
    //        LogLevel level, LoggerObjRecord loggerObjRecord);
    //}

    public partial class Program
    {
        
        static void Main(string[] args)
        {
            //    var t = new RegexBenchmark();
            //    t.RegexStaticMethod();
            //    t.RegexStaticVariable();

            //var pattern = FastRegex.GetPattern(@"\d+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            //pattern.IsMatch(
            //    "asdjkla j;ajkd;da da asdkswegredsgjvnxvn p ajk;d k;jajkdakjdn k;jbn ak;bjdnak;jbnd 123 ak;bjd ak;sbjd ask;db ;");


            //var f = ReflectionHelper.GetProperties(typeof(Test));
            //var f2 = ReflectionHelper.GetProperties(typeof(Test));

            var loggerFactory = LoggerFactory.Create(builder => builder.Services.AddSingleton<ILoggerProvider, ConsoleLoggerProvider>());
            var _logger = loggerFactory.CreateLogger(nameof(Program));

            var obj = new LoggerObj();
            var obj2 = new LoggerObjRecord(10000, "name");
            ////LoggerExtensions.ErrorStringParam(_logger, JsonSerializer.Serialize(obj), null);
            ////LoggerExtensions.ErrorOneParam(_logger, obj, null);

            ////_logger.LogError("Writing hello world response to {Param1}", obj);
            ////_logger.LogError("Writing hello world response to {Param1}", obj2);

            //Log.LogHelloWorld(_logger, LogLevel.Error, obj2);

            //try
            //{
            //    throw new Exception("test ex");
            //}
            //catch (Exception e)
            //{
            //    LoggerExtensions.ErrorOneParam(_logger, obj2, e);
            //}

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

            BenchmarkRunner.Run<LoggerBenchmark>(config);
        }
    }

}
