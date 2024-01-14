using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

using IntCode;
using IntCode.IO;

long SolvePart1(IEnumerable<string> input) {
    var vmInput = new StaticInput(new long[] { 0 });
    var output = new CollectedStdout();
    var vm = new IntVM(input.First(), vmInput, output);
    vm.Run();
    return output.Output.Single();
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    // Utils.AocMain(args, SolvePart2);
}
