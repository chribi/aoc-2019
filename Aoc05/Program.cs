using LibAoc;
using static LibAoc.LogUtils;

using IntCode;
using IntCode.IO;

int SolvePart1(IEnumerable<string> input) {
    var inp = new StaticInput(new[] { 1 });
    var output = new CollectedStdout();

    var vm = new IntVM(input.First(), inp, output);
    vm.Run();
    return output.Output.Last();
}

int SolvePart2(IEnumerable<string> input) {
    var inp = new StaticInput(new[] { 5 });
    var output = new CollectedStdout();

    var vm = new IntVM(input.First(), inp, output);
    vm.Run();
    return output.Output.Last();
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
