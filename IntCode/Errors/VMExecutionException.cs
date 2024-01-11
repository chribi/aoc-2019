namespace IntCode.Errors;

[Serializable]
public class VMExecutionException : Exception {

    public VMExecutionException() : base("Bad input") { }

    public VMExecutionException(string message) : base(message) { }

    public VMExecutionException(string message, Exception inner) : base(message, inner) { }
}
