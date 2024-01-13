namespace IntCode.IO;

public interface VMOutput {
    void Write(long number);
    void Reset();
}

public sealed class Stdout : VMOutput {
    public void Write(long number) {
        Console.WriteLine(number);
    }

    public void Reset() { }
}

public sealed class CollectOutput : VMOutput {
    public List<long> Output { get; } = new List<long>();

    public void Write(long number) {
        Output.Add(number);
    }

    public void Reset() => Output.Clear();
}

public sealed class CollectedStdout : VMOutput {
    private CollectOutput _collect = new CollectOutput();
    private Stdout _stdout = new Stdout();

    public List<long> Output => _collect.Output;

    public void Reset()
    {
        _collect.Reset();
        _stdout.Reset();
    }

    public void Write(long number)
    {
        _collect.Write(number);
        _stdout.Write(number);
    }
}
