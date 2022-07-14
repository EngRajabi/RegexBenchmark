using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace RegexBenchmark;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class X509Benchmark
{
    public static readonly byte[] file =
        File.ReadAllBytes(@"F:\Projects\BenchmarkApp\RegexBenchmark\bin\Debug\net6.0\cert.pfx");

    public static readonly X509Certificate2 _cert = new X509Certificate2(file, "123456");
    public static readonly RSA _certWithKey = new X509Certificate2(file, "123456").GetRSAPrivateKey();

    public static readonly string data =
        "eL/y4naklNsDJ4J5V0PlXQIIyY1iRG4dcjOHTqYl98nkmtbnVFg3LUgFmLU4UUVF2QQXK2RHXl8D5I3x4Y0NkboDxB6iJZj9bI9+c52ho+N7zCCvWcJTCcwFOi2XLvETQPn2trNTKbnOKqWZFRDot6dqHZlCnvxL17yLrUHj4rem62Z7ZopSmW0xi7KCnto53ctwwsYmHSxsCIjE1mzQsCOZxUv9ZEmEQUqUiuo3VipMDVrQ9txYiTBMeCpEHVCvoJIbgTBYLZyUVgZezhkgfotqsYTJeJEKk7PecrgJJ1460byrcNyW1AjnEipfmBT0QOc7r/l79DyvxwlEe2M4nA==";

    [Benchmark]
    public void X509Certificate2()
    {
        //"F:\Projects\BenchmarkApp\RegexBenchmark\bin\Debug\net6.0\cert.pfx"

        using var cert = new X509Certificate2(@"F:\Projects\BenchmarkApp\RegexBenchmark\bin\Debug\net6.0\cert.pfx", "123456");
        var decrypt = cert.GetRSAPrivateKey().Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA256);

    }

    [Benchmark]
    public void X509Certificate2File()
    {
        using var cert = new X509Certificate2(file, "123456");
        var decrypt = cert.GetRSAPrivateKey().Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA256);
    }

    [Benchmark]
    public void X509Certificate2Static()
    {
        var decrypt = _cert.GetRSAPrivateKey().Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA256);
    }

    [Benchmark]
    public void X509Certificate2StaticWithKey()
    {
        var decrypt = _certWithKey.Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA256);
    }

}
