namespace LibAoc {
    public static class Utils {
        public static IEnumerable<string> ReadLines(string fileName) {
            using var f = File.OpenRead(fileName);
            using var sr = new StreamReader(f);

            string? line;
            while((line = sr.ReadLine()) != null) {
                yield return line;
            }
        }

        public static IEnumerable<List<string>> SplitAtEmptyLines(IEnumerable<string> lines) {
            var currentBlock = new List<string>();
            foreach (var line in lines) {
                if (line == "") {
                    yield return currentBlock;
                    currentBlock = new List<string>();
                } else {
                    currentBlock.Add(line);
                }
            }
            yield return currentBlock;
        }

        public static void AocMain<TResult>(string[] args, Func<IEnumerable<string>, TResult> solve) {
            if (args.Length == 0) {
                Console.WriteLine("No input file!");
                return;
            }

            if (args.Length > 1 && args[1] == "log") {
                LogUtils.EnableLogging = true;
            }
            var lines = ReadLines(args[0]);
            var result = solve(lines);
            Console.WriteLine($"Result: {result}");
        }

        public static void AocMain<TResult>(string[] args, Func<string, TResult> solve) {
            if (args.Length == 0) {
                Console.WriteLine("No input file!");
                return;
            }

            var text = File.ReadAllText(args[0]);
            var result = solve(text);
            Console.WriteLine($"Result: {result}");
        }
    }
}
