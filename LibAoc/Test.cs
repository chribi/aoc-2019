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

    public static void AssertEqualLists<T>(IList<T> actual, IList<T> expected, string? message = null) {
        message = message ?? "AssertEqual";

        var error = "";
        if (actual.Count != expected.Count) {
            error = $"\n\tDifferent Lengths: {actual.Count} != {expected.Count}";
        } else {
            for (var i = 0; i < actual.Count; i++) {
                if (!Equals(actual[i], expected[i])) {
                    error = $"\n\tDifferent at index {i}: {actual[i]} != {expected[i]}";
                    break;
                }
            }
        }
        if (string.IsNullOrEmpty(error)) {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine($"{message}: {string.Join(",", actual)} == {string.Join(", ", expected)}{error}");

        Console.ResetColor();
    }
}
