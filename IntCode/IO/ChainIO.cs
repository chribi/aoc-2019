namespace IntCode.IO;

public sealed class ChainIO : VMBlockingInput, VMOutput {
    public Queue<int> Values { get; } = new Queue<int>();

    public void Feed(int value) {
        Values.Enqueue(value);
    }

    public bool CanRead() => Values.Any();

    public int Read() => Values.Dequeue();

    public void Reset() {
        Values.Clear();
    }

    public void Write(int number) => Feed(number);

    public override string ToString() {
        if (Values.Any()) {
            return Values.Peek().ToString();
        } else {
            return "<blocked>";
        }
    }
}
