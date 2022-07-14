using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace RegexBenchmark;

public static class FastRegex
{
    private static readonly ConcurrentDictionary<string, Regex> Regexes = new();
    private static readonly RegexOptions _defaultOption = RegexOptions.Compiled;
    private static RegexKey _lastRegex;


    public static Regex GetPattern(string pattern)
    {
        return GetRegexOption(pattern, _defaultOption);
    }

    public static Regex GetPattern(string pattern, RegexOptions options)
    {
        return GetRegexOption(pattern, _defaultOption | options);
    }

    public static Regex GetPattern(string pattern, RegexOptions options, TimeSpan timeout)
    {
        return GetRegexOption(pattern, _defaultOption | options, timeout);
    }

    private static Regex GetRegexOption(string pattern, RegexOptions options, TimeSpan? timeout = null)
    {
        if (_lastRegex.Pattern == pattern)
            return _lastRegex.Regex;

        if (Regexes.TryGetValue(pattern, out var regex))
            return regex;

        if (timeout.HasValue)
            regex = new Regex(pattern, options, timeout.Value);
        else
            regex = new Regex(pattern, options);

        Regexes.TryAdd(pattern, regex);

        //_lastRegex = new RegexKey(pattern, regex);

        //Interlocked.CompareExchange(ref _lastRegex, new RegexKey(pattern, regex), _lastRegex);

        return regex;
    }

    public readonly struct RegexKey
    {
        public readonly string Pattern;
        public readonly Regex Regex;

        public RegexKey(string pattern, Regex regex)
        {
            Pattern = pattern;
            Regex = regex;
        }
    }
}