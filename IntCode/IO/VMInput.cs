namespace IntCode.IO;

public interface VMInput {
    long Read();
    void Reset();
}

public interface VMBlockingInput : VMInput {
    bool CanRead();
}

public sealed class Stdin : VMInput {
    private readonly bool _retryOnError;
    public Stdin(bool retryOnError = true) {
        _retryOnError = retryOnError;
    }

    public long Read() {
        var input = Console.ReadLine();
        read:
        if (!long.TryParse(input, out var number)) {
            if (_retryOnError) goto read; // I use goto, sue me
            throw new Errors.BadInputException(input ?? "<null>");
        }

        return number;
    }

    public void Reset() { }
}

public sealed class StaticInput : VMInput {
    private readonly long[] _input;
    private int _pos = 0;
    public StaticInput(IEnumerable<long> input) {
        _input = input.ToArray();
    }

    public long Read()
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
