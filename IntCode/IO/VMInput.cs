namespace IntCode.IO;

public interface VMInput {
    int Read();
    void Reset();
}

public sealed class Stdin : VMInput {
    private readonly bool _retryOnError;
    public Stdin(bool retryOnError = true) {
        _retryOnError = retryOnError;
    }

    public int Read() {
        var input = Console.ReadLine();
        read:
        if (!int.TryParse(input, out var number)) {
            if (_retryOnError) goto read; // I use goto, sue me
            throw new Errors.BadInputException(input ?? "<null>");
        }

        return number;
    }

    public void Reset() { }
}

public sealed class StaticInput : VMInput {
    private readonly int[] _input;
    private int _pos = 0;
    public StaticInput(IEnumerable<int> input) {
        _input = input.ToArray();
    }

    public int Read()
    {
        if (_pos >= _input.Length) {
            throw new Errors.BadInputException($"ReadCount = {_pos + 1}", "Too many Reads!");
        }

        return _input[_pos++];
    }

    public void Reset() {
        _pos = 0;
    }
}
