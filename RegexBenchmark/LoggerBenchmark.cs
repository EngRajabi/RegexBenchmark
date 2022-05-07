using System;
using System.Collections.Generic;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    [Benchmark]
    public void ErrorStringParam()
    {
        var obj = new LoggerObjRecord(10000, "name");
        LoggerExtensions.ErrorStringParam(_logger, JsonSerializer.Serialize(obj), null);
    }

    [Benchmark]
    public void ErrorObjParam()
    {
        var obj = new LoggerObjRecord(10000, "name");
        LoggerExtensions.ErrorOneParam(_logger, obj, null);
    }

    //[Benchmark]
    //public void ErrorLevelWithCheck()
    //{
    //    LoggerExtensions.ErrorLevelWithCheck(_logger, "this log for test", null);
    //}

    //OriginalCode
    [Benchmark]
    public void OriginalCodeWithManualSerialize()
    {
        var obj = new LoggerObjRecord(10000, "name");
        _logger.LogError("Writing hello world response to {Param1}", JsonSerializer.Serialize(obj));
    }

    //OriginalCode
    [Benchmark]
    public void OriginalCode2()
    {
        var obj = new LoggerObjRecord(10000, "name");
        _logger.LogError("Writing hello world response to {Param1}", obj);
    }

    //LogHelloWorld
    [Benchmark]
    public void LogHelloWorld()
    {
        var obj = new LoggerObjRecord(10000, "name");
        LoggerExtensions.LogHelloWorld(_logger, LogLevel.Error, obj);
    }

    //LogHelloWorld2
    [Benchmark]
    public void LogHelloWorld2()
    {
        var obj = new LoggerObjRecord(10000, "name");
        _logger.LogHelloWorld2(obj);
    }

    //[Benchmark]
    //public void ErrorStringParam()
    //{
    //    LoggerExtensions.ErrorStringParam(_logger, "this log for test", null);
    //}

    //[Benchmark]
    //public void ErrorOneParam()
    //{
    //    LoggerExtensions.ErrorOneParam(_logger, "this log for test", null);
    //}

    //[Benchmark]
    //public void ErrorLevelWithCheck()
    //{
    //    LoggerExtensions.ErrorLevelWithCheck(_logger, "this log for test", null);
    //}

    ////OriginalCode
    //[Benchmark]
    //public void OriginalCode()
    //{
    //    _logger.LogError("Writing hello world response to {Param1}", "this log for test");
    //}
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