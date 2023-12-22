using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using ZLogger;

namespace RegexBenchmark;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class LoggerBenchmark
{
    private readonly ILogger _logger;

    public LoggerBenchmark()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.Services.AddSingleton<ILoggerProvider, InMemoryProvider>());
        _logger = loggerFactory.CreateLogger(nameof(LoggerBenchmark));
    }

    //OriginalCode
    [Benchmark]
    public void OriginalCodeWithManualSerialize()
    {
        var obj = new LoggerObjRecord(10000, "name adsadd sgdsdfg ererqgreagearg aeg r");
        _logger.LogError("Writing hello world response to {Param1}", JsonSerializer.Serialize(obj));
    }

    //OriginalCode
    [Benchmark]
    public void OriginalCodeWithInterpolite()
    {
        var obj = new LoggerObjRecord(10000, "name adsadd sgdsdfg ererqgreagearg aeg r");
        _logger.LogError($"Writing hello world response to {obj.Id} {obj.Name}");
    }

    //OriginalCode
    [Benchmark]
    public void OriginalCodeParam()
    {
        var obj = new LoggerObjRecord(10000, "name adsadd sgdsdfg ererqgreagearg aeg r");
        _logger.LogError("Writing hello world response to {Param1}", obj);
    }

    //LogWithSourceGenerator
    [Benchmark]
    public void LogWithSourceGenerator()
    {
        var obj = new LoggerObjRecord(10000, "name adsadd sgdsdfg ererqgreagearg aeg r");
        _logger.LogHelloWorld2(obj);
    }

    //ZLogger
    [Benchmark]
    public void ZLogger()
    {
        var obj = new LoggerObjRecord(10000, "name adsadd sgdsdfg ererqgreagearg aeg r");
        _logger.ZLogError($"Writing hello world response to {obj.Id} {obj.Name}");
    }

}

public record LoggerObjRecord(long Id, string Name);

public class LoggerObj
{
    public LoggerObj()
    {
        Ages = new List<long>();
        Ages.Add(1000);
    }

    public long Id { get; set; } = 1000000;
    public string Name { get; set; } = "ali madadi";
    public List<long> Ages { get; set; } = new();
}

public class InMemoryProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new InMemoryLogger();
    }

    public void Dispose()
    {
    }

    private class InMemoryLogger : ILogger
    {
        private int _count;

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _count++;
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();

        /// 
        public void Dispose()
        {
        }
    }
}