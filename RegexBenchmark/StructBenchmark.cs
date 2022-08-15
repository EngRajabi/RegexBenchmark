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
    public void StructReadonly()
    {
        var s = new PersonReadonly("Mohsen", "Rajabi");
        var g = s.Family;
    }

    [Benchmark]
    public void StructRecord()
    {
        var s = new PersonStructRecord("Mohsen", "Rajabi");
        var g = s.Family;
    }

    [Benchmark]
    public void ClassRecord()
    {
        var s = new PersonClassRecord("Mohsen", "Rajabi");
        var g = s.Family;
    }

    [Benchmark]
    public void ClassSealed()
    {
        var s = new PersonSealed("Mohsen", "Rajabi");
        var g = s.Family;
    }

    [Benchmark]
    public void Class()
    {
        var s = new PersonClass("Mohsen", "Rajabi");
        var g = s.Family;
    }
}

public readonly record struct PersonStructRecord(string Name, string Family);

public record class PersonClassRecord(string Name, string Family);

public readonly struct PersonReadonly
{
    public PersonReadonly(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }
}

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

public sealed class PersonSealed
{
    public PersonSealed(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }
}

public class PersonClass
{
    public PersonClass(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }
}