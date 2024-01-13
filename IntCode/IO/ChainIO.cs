namespace IntCode.IO;

public sealed class ChainIO : VMBlockingInput, VMOutput {
    public Queue<long> Values { get; } = new Queue<long>();

    public void Feed(long value) {
        Values.Enqueue(value);
    }

    public bool CanRead() => Values.Any();

    public long Read() => Values.Dequeue();

    public void Reset() {
        Values.Clear();
    }

    public void Write(long number) => Feed(number);

    public override string ToString() {
        if (Values.Any()) {
            return Values.Peek().ToString();
        } else {
            return "<blocked>";
        }
    }
}
