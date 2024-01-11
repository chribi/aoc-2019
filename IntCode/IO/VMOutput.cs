namespace IntCode.IO;

public interface VMOutput {
    void Write(int number);
    void Reset();
}

public sealed class Stdout : VMOutput {
    public void Write(int number) {
        Console.WriteLine(number);
    }

    public void Reset() { }
}

public sealed class CollectOutput : VMOutput {
    public List<int> Output { get; } = new List<int>();

    public void Write(int number) {
        Output.Add(number);
    }

    public void Reset() => Output.Clear();
}

public sealed class CollectedStdout : VMOutput {
    private CollectOutput _collect = new CollectOutput();
    private Stdout _stdout = new Stdout();

    public List<int> Output => _collect.Output;

    public void Reset()
    {
        _collect.Reset();
        _stdout.Reset();
    }

    public void Write(int number)
    {
        _collect.Write(number);
        _stdout.Write(number);
    }
}
