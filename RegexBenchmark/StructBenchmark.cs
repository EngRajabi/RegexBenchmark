using System.Threading.Tasks;
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
        s.SetDelay();
    }

    [Benchmark]
    public void StructReadonly()
    {
        var s = new PersonReadonly("Mohsen", "Rajabi");
        var g = s.Family;
        s.SetDelay();
    }

    [Benchmark]
    public void StructRecord()
    {
        var s = new PersonStructRecord("Mohsen", "Rajabi");
        var g = s.Family;
        s.SetDelay();
    }

    [Benchmark]
    public void ClassRecord()
    {
        var s = new PersonClassRecord("Mohsen", "Rajabi");
        var g = s.Family;
        s.SetDelay();
    }

    [Benchmark]
    public void ClassSealed()
    {
        var s = new PersonSealed("Mohsen", "Rajabi");
        var g = s.Family;
        s.SetDelay();
    }

    [Benchmark]
    public void Class()
    {
        var s = new PersonClass("Mohsen", "Rajabi");
        var g = s.Family;
        s.SetDelay();
    }
}

public class BasePerson
{
    public virtual Task SetDelay()
    {
        return Task.CompletedTask;
    }
}
public record class RecordBasePerson
{
    public virtual Task SetDelay()
    {
        return Task.CompletedTask;
    }
}

public readonly record struct PersonStructRecord(string Name, string Family)
{
    public Task SetDelay()
    {
        return Task.CompletedTask;
    }
}

public record class PersonClassRecord(string Name, string Family) : RecordBasePerson
{
    public override Task SetDelay()
    {
        return Task.CompletedTask;
    }
}

public readonly struct PersonReadonly
{
    public PersonReadonly(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }

    public Task SetDelay()
    {
        return Task.CompletedTask;
    }
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


    public Task SetDelay()
    {
        return Task.CompletedTask;
    }
}

public sealed class PersonSealed : BasePerson
{
    public PersonSealed(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }

    public override Task SetDelay()
    {
        return Task.CompletedTask;
    }
}

public class PersonClass : BasePerson
{
    public PersonClass(string name, string family)
    {
        Name = name;
        Family = family;
    }

    public string Name { get; init; }
    public string Family { get; init; }

    public override Task SetDelay()
    {
        return Task.CompletedTask;
    }
}