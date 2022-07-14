using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace RegexBenchmark;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class StructBenchmark
{
    [Benchmark]
    public void Struct()
    {
        var s = new Person("Mohsen", "Rajabi");
        var g = s.Family;
    }

    [Benchmark]
    public void StructRecord()
    {
        var s = new PersonRecord("Mohsen", "Rajabi");
        var g = s.Family;
    }
}
public readonly record struct PersonRecord(string Name, string Family);
public struct Person
{
    public Person(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }
}