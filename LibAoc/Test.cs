namespace LibAoc;

public static class Test {
    public static void AssertEqual<T>(T actual, T expected, string? message = null) {
        message = message ?? "AssertEqual";
        if (Equals(actual, expected)) {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine($"{message}: {actual} == {expected}");

        Console.ResetColor();
    }
}
