using System;
using Microsoft.Extensions.Logging;

namespace RegexBenchmark;

public static partial class LoggerExtensions
{
    private static readonly Action<ILogger, object, Exception?> _error_one_param =
        LoggerMessage.Define<object>(
            LogLevel.Error,
            0,
            "Writing hello world response to {Param1}");

    private static readonly Action<ILogger, string, Exception?> _error_string_param =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            1,
            "Writing hello world response to {Param1}");

    public static void ErrorStringParam(ILogger logger, string value1, Exception? exception)
    {
        _error_string_param(logger, value1, exception);
    }

    public static void ErrorOneParam(ILogger logger, object value1, Exception? exception)
    {
        _error_one_param(logger, value1, exception);
    }

    public static void ErrorLevelWithCheck(ILogger logger, string value, Exception? exception)
    {
        if (logger.IsEnabled(LogLevel.Error))
            _error_string_param(logger, value, exception);
    }

    [LoggerMessage(
        EventId = 2,
        Message = "Writing hello world response to {LoggerObjRecord}")]
    public static partial void LogHelloWorld(ILogger logger,
        LogLevel level, LoggerObjRecord loggerObjRecord);


}

public static partial class LoggerExtensions
{

    [LoggerMessage(Message = "Writing hello world response to {LoggerObjRecord}", Level = LogLevel.Error, EventId = 3)]
    public static partial void LogHelloWorld2(this ILogger logger, LoggerObjRecord loggerObjRecord);
}