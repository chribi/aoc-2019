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
    private List<int> Output { get; } = new List<int>();

    public void Write(int number) {
        Output.Add(number);
    }

    public void Reset() => Output.Clear();
}
