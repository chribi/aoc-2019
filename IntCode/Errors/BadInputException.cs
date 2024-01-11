namespace IntCode.Errors;

[Serializable]
public class BadInputException : Exception {
    public string? BadInput { get; }

    public BadInputException() : base("Bad input") { }

    public BadInputException(string input)
        : base($"Bad input '{input}'") {
        BadInput = input;
    }

    public BadInputException(string input, string reason)
        : base($"Bad input '{input}': {reason}") {
        BadInput = input;
    }

    public BadInputException(string input, string reason, Exception inner)
        : base($"Bad input '{input}': {reason}", inner) {
        BadInput = input;
    }
}
